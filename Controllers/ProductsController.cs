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

        [Authorize] // // Blocks access to users without a valid JWT
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

            Guid ProcessId = StaticData.TransactionProcesses.UpdateProduct;

            _context.OBJ_Products.Add(newProduct);
            _transactionService.SaveTransaction(ProcessId, CurrentUserId, "OBJ_Product", newProduct.Id, "A new product was created", auditDetails);            

            await _context.SaveChangesAsync();

            // We return the 201 code. The new product and the URL
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);

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

        [Authorize] // Blocks access to users without a valid JWT
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

        [Authorize] // Blocks access to users without a valid JWT
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productDto)
        {

            if (id != productDto.Id) return BadRequest("The url id does not match with data.");

            var product = await _context.OBJ_Products.FindAsync(id);

            if (product == null) return NotFound(new { message = "The product does not exit" });

            // With this we can know how was the data before changes
            var jsonDetailsBefore = new
            {
                product.Name,
                product.Description,
                product.Value,
                product.Stock,
                product.CategoryId,
                product.SubCategoryId
            };

            Guid ProcessId = StaticData.TransactionProcesses.UpdateProduct;

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Value = productDto.Value;
            product.Stock = productDto.Stock;
            product.CategoryId = productDto.CategoryId;
            product.SubCategoryId = productDto.SubCategoryId;

            var auditDetails = JsonSerializer.Serialize(new
            {
                Before = jsonDetailsBefore,
                After = new
                {
                    product.Name,
                    product.Description,
                    product.Value,
                    product.Stock,
                    product.CategoryId,
                    product.SubCategoryId
                }
            });

            _transactionService.SaveTransaction(ProcessId, CurrentUserId, "OBJ_Product", product.Id, "The product was updated", auditDetails);
            await _context.SaveChangesAsync();

            // We return the 204 code to the Frontend
            return NoContent();

        }

    }
}
