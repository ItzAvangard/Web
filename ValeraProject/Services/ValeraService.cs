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

        public async Task<ValeraDto> GetValeraAsync(int id = 1)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
            {
                valera = new Valera();
                _context.Valeras.Add(valera);
                await _context.SaveChangesAsync();
            }
            
            return ToDto(valera);
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

        public async Task<ValeraDto> ResetValeraAsync(int id = 1)
        {
            var valera = await _context.Valeras.FindAsync(id);
            if (valera == null)
            {
                valera = new Valera { Id = id };
                _context.Valeras.Add(valera);
            }
            else
            {
                var newValera = new Valera();
                UpdateValeraFromNew(valera, newValera);
            }

            await _context.SaveChangesAsync();
            return ToDto(valera);
        }

        private void UpdateValeraFromNew(Valera target, Valera source)
        {
            target.Health = source.Health;
            target.Mana = source.Mana;
            target.Cheerfulness = source.Cheerfulness;
            target.Fatigue = source.Fatigue;
            target.Money = source.Money;
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