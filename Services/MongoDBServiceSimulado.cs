using MongoDBReports.Models;

namespace MongoDBReports.Services
{
    public class MongoDBServiceSimulado
    {
        private readonly List<HabitoFitness> _habitosEnMemoria = new List<HabitoFitness>();
        private readonly ILogger<MongoDBServiceSimulado> _logger;

        public MongoDBServiceSimulado(ILogger<MongoDBServiceSimulado> logger)
        {
            _logger = logger;
            
            // Datos de ejemplo en memoria
            _habitosEnMemoria.Add(new HabitoFitness { 
                Actividad = "Running de Prueba", 
                Tipo = "Cardio",
                DuracionMinutos = 30,
                CaloriasQuemadas = 300,
                Fecha = DateTime.UtcNow.AddDays(-1),
                Intensidad = "Alta",
                Notas = "Datos de ejemplo en memoria"
            });
            
            _logger.LogInformation("Servicio simulado inicializado con datos en memoria");
        }

        public async Task<List<HabitoFitness>> GetHabitosAsync()
        {
            await Task.Delay(100); // Simular async
            return _habitosEnMemoria;
        }

        public async Task CreateHabitoAsync(HabitoFitness habito)
        {
            await Task.Delay(100); // Simular async
            habito.Id = Guid.NewGuid().ToString();
            _habitosEnMemoria.Add(habito);
            _logger.LogInformation("Hábito creado en memoria: {Actividad}", habito.Actividad);
        }

        public async Task DeleteHabitoAsync(string id)
        {
            await Task.Delay(100); // Simular async
            var habito = _habitosEnMemoria.FirstOrDefault(h => h.Id == id);
            if (habito != null)
            {
                _habitosEnMemoria.Remove(habito);
                _logger.LogInformation("Hábito eliminado de memoria: {Id}", id);
            }
        }
    }
}
