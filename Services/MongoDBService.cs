using MongoDBReports.Models;
using MongoDB.Driver;

namespace MongoDBReports.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<HabitoFitness> _habitosCollection;

        public MongoDBService()
        {
            try 
            {
                var connectionString = "mongodb+srv://damianvm:damian123@cluster0.9i09lhe.mongodb.net/ReportsDB?retryWrites=true&w=majority";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("ReportsDB");
                _habitosCollection = database.GetCollection<HabitoFitness>("habitos_fitness");
                
                // Crear datos de ejemplo de forma segura
                _ = CreateSampleDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error conectando a MongoDB: {ex.Message}");
            }
        }

        private async Task CreateSampleDataAsync()
        {
            try 
            {
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
                            Intensidad = "Alta"
                        }
                    };
                    await _habitosCollection.InsertManyAsync(sampleHabitos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando datos: {ex.Message}");
            }
        }

        public async Task<List<HabitoFitness>> GetHabitosAsync()
        {
            try 
            {
                return await _habitosCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo hábitos: {ex.Message}");
                return new List<HabitoFitness>();
            }
        }

        public async Task CreateHabitoAsync(HabitoFitness habito)
        {
            try 
            {
                await _habitosCollection.InsertOneAsync(habito);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando hábito: {ex.Message}");
            }
        }

        public async Task DeleteHabitoAsync(string id)
        {
            try 
            {
                await _habitosCollection.DeleteOneAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando hábito: {ex.Message}");
            }
        }
    }
}
