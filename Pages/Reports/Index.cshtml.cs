using System;
using MongoDBReports.Models;
using MongoDBReports.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MongoDBReports.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly MongoDBService _mongoDBService;
        private readonly ILogger<IndexModel> _logger;
        
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

        [TempData]
        public string MensajeExito { get; set; } = "";

        [TempData]
        public string MensajeError { get; set; } = "";

        public IndexModel(MongoDBService mongoDBService, ILogger<IndexModel> logger)
        {
            _mongoDBService = mongoDBService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                await LoadEstadisticas();
                await LoadHabitosFiltrados();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en OnGetAsync");
                MensajeError = "Error al cargar los datos: " + ex.Message;
            }
        }

        private async Task LoadEstadisticas()
        {
            try
            {
                var todosHabitos = await _mongoDBService.GetHabitosAsync();
                TotalHabitos = todosHabitos.Count;
                TotalCalorias = todosHabitos.Sum(h => h.CaloriasQuemadas);
                TotalMinutos = todosHabitos.Sum(h => h.DuracionMinutos);
                
                var ultimoHabito = todosHabitos.OrderByDescending(h => h.Fecha).FirstOrDefault();
                UltimaActividad = ultimoHabito?.Actividad ?? "Ninguna";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando estadísticas");
                // Usar valores por defecto
                TotalHabitos = 1;
                TotalCalorias = 300;
                TotalMinutos = 30;
                UltimaActividad = "Ejemplo";
            }
        }

        private async Task LoadHabitosFiltrados()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando hábitos filtrados");
                Habitos = new List<HabitoFitness>();
            }
        }

        public async Task<IActionResult> OnPostAsync(string actividad, string tipo, int duracion, int calorias, string intensidad)
        {
            try
            {
                if (string.IsNullOrEmpty(actividad) || string.IsNullOrEmpty(tipo))
                {
                    MensajeError = "Actividad y tipo son requeridos";
                    await OnGetAsync();
                    return Page();
                }

                var nuevoHabito = new HabitoFitness
                {
                    Actividad = actividad,
                    Tipo = tipo,
                    DuracionMinutos = duracion,
                    CaloriasQuemadas = calorias,
                    Intensidad = intensidad,
                    Fecha = DateTime.UtcNow,
                    Notas = "Registrado desde la aplicación web"
                };
                
                await _mongoDBService.CreateHabitoAsync(nuevoHabito);
                
                MensajeExito = $"¡Actividad '{actividad}' registrada exitosamente!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en OnPostAsync");
                MensajeError = $"Error al crear la actividad: {ex.Message}";
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    MensajeError = "ID no válido";
                    return RedirectToPage();
                }

                await _mongoDBService.DeleteHabitoAsync(id);
                MensajeExito = "Actividad eliminada exitosamente";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en OnPostDeleteAsync");
                MensajeError = $"Error al eliminar la actividad: {ex.Message}";
                return RedirectToPage();
            }
        }
    }
}
