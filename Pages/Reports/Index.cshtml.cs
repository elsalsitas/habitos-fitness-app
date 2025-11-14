using System;
using MongoDBReports.Models;
using MongoDBReports.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;

namespace MongoDBReports.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;
        
        public List<HabitoFitness> Habitos { get; set; } = new List<HabitoFitness>();
        public int TotalHabitos { get; set; }
        public int TotalCalorias { get; set; }
        public int TotalMinutos { get; set; }
        public string UltimaActividad { get; set; } = "Ninguna";
        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = "";
        
        [BindProperty(SupportsGet = true)]
        public string TipoSeleccionado { get; set; } = "";
        
        [BindProperty(SupportsGet = true)]
        public string IntensidadSeleccionada { get; set; } = "";

        public IndexModel(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task OnGetAsync()
        {
            await LoadEstadisticas();
            await LoadHabitosFiltrados();
        }

        private async Task LoadEstadisticas()
        {
            var todosHabitos = await _mongoDBService.GetHabitosAsync();
            TotalHabitos = todosHabitos.Count;
            TotalCalorias = todosHabitos.Sum(h => h.CaloriasQuemadas);
            TotalMinutos = todosHabitos.Sum(h => h.DuracionMinutos);
            
            var ultimoHabito = todosHabitos.OrderByDescending(h => h.Fecha).FirstOrDefault();
            UltimaActividad = ultimoHabito?.Actividad ?? "Ninguna";
        }

        private async Task LoadHabitosFiltrados()
        {
            var todosHabitos = await _mongoDBService.GetHabitosAsync();
            
            // Aplicar filtros
            var filtrados = todosHabitos.AsQueryable();
            
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filtrados = filtrados.Where(h => 
                    h.Actividad.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    h.Notas.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrEmpty(TipoSeleccionado))
            {
                filtrados = filtrados.Where(h => h.Tipo == TipoSeleccionado);
            }
            
            if (!string.IsNullOrEmpty(IntensidadSeleccionada))
            {
                filtrados = filtrados.Where(h => h.Intensidad == IntensidadSeleccionada);
            }
            
            // Ordenar por fecha más reciente
            Habitos = filtrados.OrderByDescending(h => h.Fecha).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string actividad, string tipo, int duracion, int calorias, string intensidad)
        {
            if (!string.IsNullOrEmpty(actividad) && !string.IsNullOrEmpty(tipo))
            {
                var nuevoHabito = new HabitoFitness
                {
                    Actividad = actividad,
                    Tipo = tipo,
                    DuracionMinutos = duracion,
                    CaloriasQuemadas = calorias,
                    Intensidad = intensidad,
                    Fecha = DateTime.UtcNow,
                    Notas = "Actividad registrada desde la app"
                };
                
                await _mongoDBService.CreateHabitoAsync(nuevoHabito);
                
                return RedirectToPage(new { created = true });
            }
            
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _mongoDBService.DeleteHabitoAsync(id);
            }
            
            return RedirectToPage();
        }
    }
}
