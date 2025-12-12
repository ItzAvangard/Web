using ValeraProject.Models;
using ValeraProject.DTOs;

namespace ValeraProject.Services
{
    public interface IValeraService
    {
        Task<List<ValeraDto>> GetAllValerasAsync();
        Task<List<ValeraDto>> GetMyValerasAsync(int userId);
        Task<ValeraDto?> GetValeraAsync(int id = 1);
        Task<ValeraDto> CreateValeraAsync(CreateValeraDto createDto, int userId);
        Task<ValeraDto> ExecuteActionAsync(int id, string action, int userId, string userRole);
        Task<bool> DeleteValeraAsync(int id = 1, int userId = 0, string userRole = "User");
        Task<(ValeraDto valera, bool wasCreated)> PutValeraAsync(int id, int userId, string userRole);
        Task<bool> CanAccessValeraAsync(int valeraId, int userId, string userRole);
    }
}