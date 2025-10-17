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
        public async Task<ActionResult<ValeraDto>> Get()
        {
            try
            {
                var valera = await _valeraService.GetValeraAsync();
                return Ok(valera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("action")]
        public async Task<ActionResult<ValeraDto>> ExecuteAction([FromBody] ActionRequestDto request)
        {
            try
            {
                var valera = await _valeraService.ExecuteActionAsync(1, request.Action);
                return Ok(valera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset")]
        public async Task<ActionResult<ValeraDto>> Reset()
        {
            try
            {
                var valera = await _valeraService.ResetValeraAsync();
                return Ok(valera);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}