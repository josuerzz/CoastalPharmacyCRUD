using CoastalPharmacyCRUD.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // We need to access to the User info
using Microsoft.AspNetCore.Authorization; // We need to access to the User info
using System.Text.Json;
using System.Diagnostics;
using CoastalPharmacyCRUD.Interfaces;
using CoastalPharmacyCRUD.DTOs;
using CoastalPharmacyCRUD.Models;
using CoastalPharmacyCRUD.Constants;

namespace CoastalPharmacyCRUD.Controllers
{

    /* Here we are going to add and modify Identifiers */

    [Route("api/[controller]")]
    [ApiController]
    public class IdentifiersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public IdentifiersController(ApplicationDbContext context, ITransactionService transactionService)
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
                    throw new UnauthorizedAccessException("User Id not found in Token");

                return Guid.Parse(userId);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateIdentifier([FromBody] IdentifierCreateDto identifierDto)
        {
            if (identifierDto == null) return BadRequest("Hey, there is no data here.");

            if (identifierDto.ParentId != null)
            {
                var existsParent = await _context.CDL_Identifiers.AnyAsync(p => p.Id == identifierDto.ParentId);
                if (!existsParent) return BadRequest("Hey, your information is not valid.");
            }


            var currentMax = await _context.CDL_Identifiers
            .Where(x => x.Set == identifierDto.Set)
            .Select(x => (int?)x.ElementNumber)
            .MaxAsync();

            int nextNumber = (currentMax ?? 0) + 1;
            string codeIdentifier = $"{identifierDto.Set}{nextNumber}";

            var newIdentifier = new CDL_Identifier
            {
                Id = Guid.NewGuid(),
                Set = identifierDto.Set,
                ElementNumber = nextNumber,
                Code = codeIdentifier,
                Description = identifierDto.Description,
                Use = identifierDto.Use,
                ParentId = identifierDto.ParentId
            };

            var auditDetails = JsonSerializer.Serialize(new
            {
                newIdentifier.Id,
                newIdentifier.Set,
                newIdentifier.ElementNumber,
                newIdentifier.Code,
                newIdentifier.Description,
                newIdentifier.Use
            });
            string identifierSet = identifierDto.Set;

            var (processId, descriptionProcess) = StaticData.AuditMatrix.GetValueOrDefault(
                (identifierSet, ActionType.Create),
                (StaticData.TransactionProcesses.CreateNewIdentifier, "A new Identifier was added")
                );

            _context.CDL_Identifiers.Add(newIdentifier);
            _transactionService.SaveTransaction(processId, CurrentUserId, "CDL_Identifier", newIdentifier.Id, descriptionProcess, auditDetails);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateIdentifier), new { id = newIdentifier.Id }, newIdentifier);
        }
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateIdentifier(Guid id, [FromBody] IdentifierUpdateDto identifierDto)
        {

            if (id != identifierDto.Id) return BadRequest("The url id does not match with data.");

            var identifier = await _context.CDL_Identifiers.FindAsync(id);

            if (identifier == null) return NotFound(new { message = "The identifier does not exit" });


            var jsonDetailsBefore = new
            {
                identifier.Description
            };

            identifier.Description = identifierDto.Description;

            var auditDetails = JsonSerializer.Serialize(new
            {
                Before = jsonDetailsBefore,
                After = new
                {
                    identifier.Description
                }
            });

            var (processId, descriptionProcess) = StaticData.AuditMatrix.GetValueOrDefault(
                (identifierDto.Set, ActionType.Update),
                (StaticData.TransactionProcesses.UpdateIdentifier, "Identifier updated")
                );

            _transactionService.SaveTransaction(processId, CurrentUserId, "CDL_Identifier", id, descriptionProcess, auditDetails);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
