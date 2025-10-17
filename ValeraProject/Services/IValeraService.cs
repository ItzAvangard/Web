using ValeraProject.Models;
using ValeraProject.DTOs;

namespace ValeraProject.Services
{
    public interface IValeraService
    {
        Task<ValeraDto> GetValeraAsync(int id = 1);
        Task<ValeraDto> ExecuteActionAsync(int id, string action);
        Task<ValeraDto> ResetValeraAsync(int id = 1);
    }
}