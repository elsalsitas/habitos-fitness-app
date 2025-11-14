using MongoDBReports.Models;
using MongoDB.Driver;

namespace MongoDBReports.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<HabitoFitness>? _habitosCollection;
        private readonly ILogger<MongoDBService> _logger;

        public MongoDBService(ILogger<MongoDBService> logger)
        {
            _logger = logger;
            try 
            {
                var connectionString = "mongodb+srv://damianvm:damian123@cluster0.9i09lhe.mongodb.net/ReportsDB?retryWrites=true&w=majority&tls=false&ssl=false";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("ReportsDB");
                _habitosCollection = database.GetCollection<HabitoFitness>("habitos_fitness");
                
                _logger.LogInformation("MongoDB Service inicializado");
                
                // Crear datos de ejemplo de forma asincrona
                _ = Task.Run(async () => await CreateSampleDataAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inicializando MongoDB Service");
            }
        }

        private async Task CreateSampleDataAsync()
        {
            try 
            {
                if (_habitosCollection == null) return;
                
                var count = await _habitosCollection.CountDocumentsAsync(FilterDefinition<HabitoFitness>.Empty);
                if (count == 0)
                {
                    var sampleHabitos = new List<HabitoFitness>
                    {
                        new HabitoFitness { 
                            Actividad = "Running", 
                            Tipo = "Cardio",
                            DuracionMinutos = 30,
                            CaloriasQuemadas = 300,
                            Fecha = DateTime.UtcNow.AddDays(-1),
                            Intensidad = "Alta",
                            Notas = "Datos de ejemplo"
                        }
                    };
                    await _habitosCollection.InsertManyAsync(sampleHabitos);
                    _logger.LogInformation("Datos de ejemplo creados");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando datos de ejemplo");
            }
        }

        public async Task<List<HabitoFitness>> GetHabitosAsync()
        {
            try 
            {
                if (_habitosCollection == null)
                {
                    _logger.LogWarning("Colección no disponible, devolviendo datos de ejemplo");
                    return GetDatosEjemplo();
                }
                
                return await _habitosCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo hábitos");
                return GetDatosEjemplo();
            }
        }

        private List<HabitoFitness> GetDatosEjemplo()
        {
            return new List<HabitoFitness>
            {
                new HabitoFitness { 
                    Actividad = "Ejemplo - Conexión falló", 
                    Tipo = "Cardio",
                    DuracionMinutos = 30,
                    CaloriasQuemadas = 300,
                    Fecha = DateTime.UtcNow,
                    Intensidad = "Moderada",
                    Notas = "Datos de ejemplo por fallo de conexión"
                }
            };
        }

        public async Task CreateHabitoAsync(HabitoFitness habito)
        {
            try 
            {
                if (_habitosCollection == null)
                {
                    _logger.LogWarning("No se puede crear hábito - colección no disponible");
                    throw new Exception("Servicio de base de datos no disponible");
                }
                
                await _habitosCollection.InsertOneAsync(habito);
                _logger.LogInformation("Hábito creado: {Actividad}", habito.Actividad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando hábito: {Actividad}", habito.Actividad);
                throw new Exception($"No se pudo crear el hábito: {ex.Message}");
            }
        }

        public async Task DeleteHabitoAsync(string id)
        {
            try 
            {
                if (_habitosCollection == null)
                {
                    _logger.LogWarning("No se puede eliminar hábito - colección no disponible");
                    throw new Exception("Servicio de base de datos no disponible");
                }
                
                await _habitosCollection.DeleteOneAsync(x => x.Id == id);
                _logger.LogInformation("Hábito eliminado: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando hábito: {Id}", id);
                throw new Exception($"No se pudo eliminar el hábito: {ex.Message}");
            }
        }
    }
}
