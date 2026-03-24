using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoastalPharmacyCRUD.Data;
using CoastalPharmacyCRUD.DTOs;
using CoastalPharmacyCRUD.Models;
using CoastalPharmacyCRUD.Constants;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // We need to access to the User info
using Microsoft.AspNetCore.Authorization; // We need to access to the User info
using System.Text.Json;
using System.Diagnostics;
using CoastalPharmacyCRUD.Interfaces;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using ClosedXML.Excel;

namespace CoastalPharmacyCRUD.Controllers
{
    /* Here we are going to add, modify, deactivate and get products */

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public ProductsController(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        private Guid CurrentUserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User id not found in Token");

                return Guid.Parse(userId);
            }
        }

        [Authorize(Roles = "admin")] // // Blocks access to users without a valid JWT
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        
        {
            if (productDto == null) return BadRequest("Hey, there is no data here");

            var existsCategory = await _context.CDL_Identifiers.AnyAsync(c => c.Id == productDto.CategoryId);
            if (!existsCategory) { return BadRequest("The Category doesn't even exists."); }

            var existsSubcategory = await _context.CDL_Identifiers.AnyAsync(s => s.Id == productDto.SubCategoryId);
            if (!existsSubcategory) { return BadRequest("The selected SubCategory is invalid or doesn't belong to the category."); }

            var newProduct = new OBJ_Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                CategoryId = productDto.CategoryId,
                SubCategoryId = productDto.SubCategoryId,
                Stock = productDto.Stock,
                Description = productDto.Description,
                Value = productDto.Value,
                Status = 1,
                CreateDate = DateTime.UtcNow,
                CreateUserId = CurrentUserId
            };

            var auditDetails = JsonSerializer.Serialize(new
            {
                newProduct.Name,
                newProduct.CategoryId,
                productDto.SubCategoryId,
                newProduct.Stock,
                newProduct.Description,
                newProduct.Value
            });

            Guid ProcessId = StaticData.TransactionProcesses.CreateProduct;

            _context.OBJ_Products.Add(newProduct);
            _transactionService.SaveTransaction(ProcessId, CurrentUserId, "OBJ_Product", newProduct.Id, "A new product was created", auditDetails);            

            await _context.SaveChangesAsync();

            // We return the 201 code. The new product and the URL
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);

        }
        
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var product = await _context.OBJ_Products            
            .Where(p => p.Status == 1)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Value,
                p.Description,
                CategoryName = p.Category.Description, 
                SubCategoryName = p.SubCategory.Description
            })
            .OrderBy(p => p.CategoryName)
            .ThenBy(p => p.Name)
            .ToListAsync();

            return Ok(product);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _context.OBJ_Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "The product doesn't exist." });
            }

            return Ok(product);
        }

        [Authorize(Roles = "admin")] // Blocks access to users without a valid JWT
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateProduct(Guid id)
        {

            var product = await _context.OBJ_Products.FindAsync(id);

            if (product == null) return NotFound(new { message = "The product does not exit" });

            // With this we can know how was the data before changes
            var jsonDetailsBefore = new
            {
                product.Status
            };

            // We only deactivate the product because the History and Good practice
            product.Status = 0;

            var auditDetails = JsonSerializer.Serialize(new
            {
                Before = jsonDetailsBefore,
                After = new
                {
                    product.Status
                }
            });

            Guid ProcessId = StaticData.TransactionProcesses.DeleteProduct;
            _transactionService.SaveTransaction(ProcessId, CurrentUserId, "OBJ_Product", product.Id, "The product was deactivate", auditDetails);

            await _context.SaveChangesAsync();

            // We return the 204 code to the Frontend
            return NoContent();

        }

        [Authorize(Roles = "admin")] // Blocks access to users without a valid JWT
        [HttpPut("updateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productDto)
        {

            if (id != productDto.Id) return BadRequest("The url id does not match with data.");

            var product = await _context.OBJ_Products.FindAsync(id);

            if (product == null) return NotFound(new { message = "The product does not exit." });

            if (productDto.Stock < 0 || productDto.Value < 0) 
                return BadRequest(new { message = "Stock and Value cannot be negative." });

            // With this we can know how was the data before changes
            var jsonDetailsBefore = new
            {
                product.Name,
                product.Description,
                product.Value,
                product.Stock
                // product.CategoryId,
                // product.SubCategoryId
            };

            Guid ProcessId = StaticData.TransactionProcesses.UpdateProduct;

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Value = productDto.Value;
            product.Stock = productDto.Stock;
            // product.CategoryId = productDto.CategoryId;
            // product.SubCategoryId = productDto.SubCategoryId;

            var auditDetails = JsonSerializer.Serialize(new
            {
                Before = jsonDetailsBefore,
                After = new
                {
                    product.Name,
                    product.Description,
                    product.Value,
                    product.Stock
                    // product.CategoryId,
                    // product.SubCategoryId
                }
            });

            _transactionService.SaveTransaction(ProcessId, CurrentUserId, "OBJ_Product", product.Id, "The product was updated", auditDetails);
            await _context.SaveChangesAsync();

            // We return the 204 code to the Frontend
            return NoContent();

        }

        [HttpGet("export")]
        public IActionResult ExportProductsToExcel()
        {
            var products = _context.OBJ_Products
            .Where(p => p.Status == 1)
            .Select(p => new
            {
                Name = p.Name,
                Category = p.Category.Description,
                SubCategory = p.SubCategory.Description,
                Stock = p.Stock,
                Value = p.Value,
                Description = p.Description
            })
            .ToList();

            using(var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");

                var table = worksheet.Cell(1, 1).InsertTable(products);

                // 2. PERSONALIZAR TÍTULOS (Encabezados)
                // Accedemos a la primera fila de la tabla (los headers)
                var headerRow = table.HeadersRow();

                // --- ESTILO DE FUENTE Y TAMAÑO ---
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Font.FontSize = 16;
                headerRow.Style.Font.FontName = "Verdana"; // O la de tu preferencia
                headerRow.Style.Font.FontColor = XLColor.White;

                // --- COLOR DE FONDO (Relleno) ---
                // Puedes usar colores predefinidos o códigos Hexadecimales
                headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#00a2e8"); 

                // --- ALINEACIÓN ---
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                worksheet.Column(5).Style.NumberFormat.Format = "$#,##0";

                worksheet.Columns().AdjustToContents();

                // Example: products_2026-03-07_21-00.xlsx
                var fileName = $"products_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";

                using(var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                        fileName
                        );
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreateProducts([FromBody] BulkCreateProductDto bulkProductsDto)
        {
            if (bulkProductsDto == null || !bulkProductsDto.products.Any()) return BadRequest("No data provided");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newProducts = bulkProductsDto.products.Select(p => new OBJ_Product
                {
                    Id = Guid.NewGuid(),
                    Name = p.Name,
                    Description = p.Description,
                    Value = p.Value,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    SubCategoryId = p.SubCategoryId,
                    Status = 1,
                    CreateDate = DateTime.UtcNow,
                    CreateUserId = CurrentUserId
                }).ToList();

                _context.OBJ_Products.AddRange(newProducts);

                var newImportHistory = new ImportHistory
                {
                    FileName = bulkProductsDto.fileName,
                    TotalRecords = newProducts.Count,
                    ImportDate = DateTime.UtcNow,
                    ImportStatus = CodeImportStatus.Success,
                    ImportUserId = CurrentUserId,
                    ErrorMessage = null
                };

                _context.ImportHistories.Add(newImportHistory);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new {message = $"{newProducts.Count} products imported successfully."});
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
