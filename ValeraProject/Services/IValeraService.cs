using ValeraProject.Models;
using ValeraProject.DTOs;

namespace ValeraProject.Services
{
    public interface IValeraService
    {
        Task<List<ValeraDto>> GetAllValerasAsync();
        Task<ValeraDto?> GetValeraAsync(int id = 1);
        Task<ValeraDto> CreateValeraAsync(CreateValeraDto createDto);
        Task<ValeraDto> ExecuteActionAsync(int id, string action);
        Task<bool> DeleteValeraAsync(int id = 1);
        Task<(ValeraDto valera, bool wasCreated)> PutValeraAsync(int id);
    }
}