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

        public async Task<ValeraDto?> GetValeraAsync(int id = 1)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
            {
                return null;
            }
            
            return ToDto(valera);
        }

        public async Task<(ValeraDto valera, bool wasCreated)> PutValeraAsync(int id)
        {
            var existingValera = await _context.Valeras.FindAsync(id);
            bool wasCreated = false;

            if (existingValera == null)
            {
                var newValera = new Valera { Id = id };
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

        public async Task<ValeraDto> ExecuteActionAsync(int id, string action)
        {
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

        public async Task<bool> DeleteValeraAsync(int id = 1)
        {
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
                Health = valera.Health,
                Mana = valera.Mana,
                Cheerfulness = valera.Cheerfulness,
                Fatigue = valera.Fatigue,
                Money = valera.Money
            };
        }
    }
}