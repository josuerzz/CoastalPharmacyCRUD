using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoastalPharmacyCRUD.Data;

namespace CoastalPharmacyCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDbController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestDbController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("check")]
        public IActionResult CheckConnection()
        {
            try
            {
                bool isConnected = _context.Database.CanConnect();

                if (isConnected) 
                {
                    return Ok(new { message = "Congrats, the connection is ready!", status = "Ready" });
                }
                else
                {
                    return BadRequest(new { message = "There was something wrong, can't connect" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error, look the message", Error = ex.Message });
                throw;
            }
        }

    }
}
