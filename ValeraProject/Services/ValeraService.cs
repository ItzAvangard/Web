using ValeraProject.Data;
using ValeraProject.Models;
using ValeraProject.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ValeraProject.Services
{
    public class ValeraService : IValeraService
    {
        private readonly AppDbContext _context;

        public ValeraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ValeraDto>> GetAllValerasAsync()
        {
            var valeras = await _context.Valeras.ToListAsync();
            return valeras.Select(v => ToDto(v)).ToList();
        }

        public async Task<List<ValeraDto>> GetMyValerasAsync(int userId)
        {
            var valeras = await _context.Valeras
                .Where(v => v.UserId == userId)
                .ToListAsync();
            return valeras.Select(v => ToDto(v)).ToList();
        }

        public async Task<ValeraDto?> GetValeraAsync(int id = 1)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
            {
                return null;
            }
            
            return ToDto(valera);
        }

        public async Task<ValeraDto> CreateValeraAsync(CreateValeraDto createDto, int userId)
        {
            // Находим максимальный Id и добавляем 1
            int maxId = 0;
            if (await _context.Valeras.AnyAsync())
            {
                maxId = await _context.Valeras.MaxAsync(v => v.Id);
            }
            
            var valera = new Valera
            {
                Id = maxId + 1,
                Name = createDto.Name ?? "Valera",
                Health = createDto.Health,
                Mana = createDto.Mana,
                Cheerfulness = createDto.Cheerfulness,
                Fatigue = createDto.Fatigue,
                Money = createDto.Money,
                UserId = userId
            };
            
            _context.Valeras.Add(valera);
            await _context.SaveChangesAsync();
            
            return ToDto(valera);
        }

        public async Task<bool> CanAccessValeraAsync(int valeraId, int userId, string userRole)
        {
            var valera = await _context.Valeras.FindAsync(valeraId);
            if (valera == null)
                return false;

            // Админ может получить доступ к любой Валере
            if (userRole == "Admin")
                return true;

            // Пользователь может получить доступ только к своей Валере
            return valera.UserId == userId;
        }

        public async Task<(ValeraDto valera, bool wasCreated)> PutValeraAsync(int id, int userId, string userRole)
        {
            // Проверка прав доступа
            if (!await CanAccessValeraAsync(id, userId, userRole))
            {
                throw new UnauthorizedAccessException("You don't have permission to access this Valera");
            }

            var existingValera = await _context.Valeras.FindAsync(id);
            bool wasCreated = false;

            if (existingValera == null)
            {
                var newValera = new Valera { Id = id, UserId = userId };
                _context.Valeras.Add(newValera);
                await _context.SaveChangesAsync();
                wasCreated = true;
                return (ToDto(newValera), wasCreated);
            }
            else
            {
                var defaultValera = new Valera();
                existingValera.Health = defaultValera.Health;
                existingValera.Mana = defaultValera.Mana;
                existingValera.Cheerfulness = defaultValera.Cheerfulness;
                existingValera.Fatigue = defaultValera.Fatigue;
                existingValera.Money = defaultValera.Money;
                await _context.SaveChangesAsync();
                return (ToDto(existingValera), wasCreated);
            }
        }

        public async Task<ValeraDto> ExecuteActionAsync(int id, string action, int userId, string userRole)
        {
            // Проверка прав доступа
            if (!await CanAccessValeraAsync(id, userId, userRole))
            {
                throw new UnauthorizedAccessException("You don't have permission to execute actions on this Valera");
            }

            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
                throw new ArgumentException("Valera not found");

            bool success = false;
            string actionLower = action.ToLower();

            switch (actionLower)
            {
                case "work":
                    success = valera.GoToWork();
                    break;
                case "nature":
                    valera.ContemplateNature();
                    success = true;
                    break;
                case "tv":
                    success = valera.DrinkWineAndWatchTV();
                    break;
                case "bar":
                    success = valera.GoToBar();
                    break;
                case "marginals":
                    success = valera.DrinkWithMarginals();
                    break;
                case "sing":
                    valera.SingInMetro();
                    success = true;
                    break;
                case "sleep":
                    valera.Sleep();
                    success = true;
                    break;
                default:
                    success = false;
                    break;
            }

            if (!success)
                throw new InvalidOperationException($"Action '{action}' cannot be executed");

            await _context.SaveChangesAsync();
            return ToDto(valera);
        }

        public async Task<bool> DeleteValeraAsync(int id = 1, int userId = 0, string userRole = "User")
        {
            // Проверка прав доступа
            if (!await CanAccessValeraAsync(id, userId, userRole))
            {
                throw new UnauthorizedAccessException("You don't have permission to delete this Valera");
            }

            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
                return false;

            _context.Valeras.Remove(valera);
            await _context.SaveChangesAsync();
            return true;
        }

        private ValeraDto ToDto(Valera valera)
        {
            return new ValeraDto
            {
                Id = valera.Id,
                Name = valera.Name,
                Health = valera.Health,
                Mana = valera.Mana,
                Cheerfulness = valera.Cheerfulness,
                Fatigue = valera.Fatigue,
                Money = valera.Money
            };
        }
    }
}