using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ValeraProject.Services;
using ValeraProject.DTOs;

namespace ValeraProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ValeraController : ControllerBase
    {
        private readonly IValeraService _valeraService;

        public ValeraController(IValeraService valeraService)
        {
            _valeraService = valeraService;
        }

        // Получить все Валеры (только для админа)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var valeras = await _valeraService.GetAllValerasAsync();
                return Ok(valeras);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Получить свои Валеры
        [HttpGet("my")]
        public async Task<ActionResult> GetMy()
        {
            try
            {
                var userId = GetUserId();
                var valeras = await _valeraService.GetMyValerasAsync(userId);
                return Ok(valeras);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Получить конкретную Валеру по ID
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var userId = GetUserId();
                var userRole = GetUserRole();
                
                // Проверка прав доступа
                if (!await _valeraService.CanAccessValeraAsync(id, userId, userRole))
                {
                    return StatusCode(403, "You don't have permission to access this Valera");
                }

                var valera = await _valeraService.GetValeraAsync(id);
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

        // Создать Валеру
        [HttpPost]
        public async Task<ActionResult<ValeraDto>> Create([FromBody] CreateValeraDto createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return BadRequest("Request body is required");
                }
                
                var userId = GetUserId();
                var valera = await _valeraService.CreateValeraAsync(createDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = valera.Id }, valera);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating Valera: {ex.Message}");
            }
        }

        // Обновить Валеру (сброс к дефолтным значениям)
        [HttpPut("{id}")]
        public async Task<ActionResult<ValeraDto>> Put(int id)
        {
            try
            {
                var userId = GetUserId();
                var userRole = GetUserRole();
                
                var (valera, wasCreated) = await _valeraService.PutValeraAsync(id, userId, userRole);
                if (wasCreated)
                {
                    return CreatedAtAction(nameof(GetById), new { id = valera.Id }, valera);
                }
                return Ok(valera);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Выполнить действие с Валерой
        [HttpPost("{id}/action")]
        public async Task<ActionResult<ValeraDto>> ExecuteAction(int id, [FromBody] ActionRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var userRole = GetUserRole();
                
                var valera = await _valeraService.ExecuteActionAsync(id, request.Action, userId, userRole);
                return Ok(valera);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Удалить Валеру
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();
                var userRole = GetUserRole();
                
                var deleted = await _valeraService.DeleteValeraAsync(id, userId, userRole);
                if (deleted)
                {
                    return NoContent();
                }
                return NotFound("Valera not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Вспомогательные методы для получения данных пользователя из токена
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }

        private string GetUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
        }
    }
}