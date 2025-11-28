using Microsoft.AspNetCore.Mvc;
using ValeraProject.Services;
using ValeraProject.DTOs;

namespace ValeraProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValeraController : ControllerBase
    {
        private readonly IValeraService _valeraService;

        public ValeraController(IValeraService valeraService)
        {
            _valeraService = valeraService;
        }

        [HttpGet]
        public async Task<ActionResult<ValeraDto>> Get([FromQuery] int? id = null)
        {
            try
            {
                var valera = await _valeraService.GetValeraAsync(id ?? 1);
                if (valera == null)
                {
                    return NotFound("Valera not found");
                }
                return Ok(valera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ValeraDto>> Put([FromQuery] int? id = null)
        {
            try
            {
                var (valera, wasCreated) = await _valeraService.PutValeraAsync(id ?? 1);
                if (wasCreated)
                {
                    return CreatedAtAction(nameof(Get), new { id = valera.Id }, valera);
                }
                return Ok(valera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("action")]
        public async Task<ActionResult<ValeraDto>> ExecuteAction([FromBody] ActionRequestDto request, [FromQuery] int? id = null)
        {
            try
            {
                var valera = await _valeraService.ExecuteActionAsync(id ?? 1, request.Action);
                return Ok(valera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int? id = null)
        {
            try
            {
                var deleted = await _valeraService.DeleteValeraAsync(id ?? 1);
                if (deleted)
                {
                    return NoContent();
                }
                return NotFound("Valera not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}