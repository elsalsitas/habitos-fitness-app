using MongoDBReports.Models;
using MongoDB.Driver;

namespace MongoDBReports.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<HabitoFitness> _habitosCollection;

        public MongoDBService()
        {
<<<<<<< HEAD
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
=======
            var connectionString = "mongodb+srv://damianvm:damian123@cluster0.9i09lhe.mongodb.net/ReportsDB?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ReportsDB");
            _habitosCollection = database.GetCollection<HabitoFitness>("habitos_fitness");
            CreateSampleDataAsync().Wait();
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
        }

        private async Task CreateSampleDataAsync()
        {
<<<<<<< HEAD
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
=======
            var count = await _habitosCollection.CountDocumentsAsync(FilterDefinition<HabitoFitness>.Empty);
            if (count == 0)
            {
                var sampleHabitos = new List<HabitoFitness>
                {
                    new HabitoFitness { 
                        Actividad = "Running en el parque", 
                        Tipo = "Cardio",
                        DuracionMinutos = 45,
                        CaloriasQuemadas = 350,
                        Fecha = DateTime.UtcNow.AddDays(-2),
                        Intensidad = "Alta",
                        Notas = "Buen ritmo, 5km completados"
                    },
                    new HabitoFitness { 
                        Actividad = "Yoga matutino", 
                        Tipo = "Flexibilidad",
                        DuracionMinutos = 30,
                        CaloriasQuemadas = 120,
                        Fecha = DateTime.UtcNow.AddDays(-1),
                        Intensidad = "Moderada",
                        Notas = "Sesión de yoga para flexibilidad"
                    },
                    new HabitoFitness { 
                        Actividad = "Entrenamiento con pesas", 
                        Tipo = "Fuerza",
                        DuracionMinutos = 60,
                        CaloriasQuemadas = 280,
                        Fecha = DateTime.UtcNow.AddHours(-5),
                        Intensidad = "Alta",
                        Notas = "Piernas y core"
                    },
                    new HabitoFitness { 
                        Actividad = "Natación", 
                        Tipo = "Cardio",
                        DuracionMinutos = 40,
                        CaloriasQuemadas = 300,
                        Fecha = DateTime.UtcNow.AddDays(-3),
                        Intensidad = "Moderada",
                        Notas = "20 piscinas completadas"
                    },
                    new HabitoFitness { 
                        Actividad = "Ciclismo", 
                        Tipo = "Cardio",
                        DuracionMinutos = 90,
                        CaloriasQuemadas = 450,
                        Fecha = DateTime.UtcNow.AddDays(-5),
                        Intensidad = "Alta",
                        Notas = "Ruta de 25km"
                    }
                };
                
                await _habitosCollection.InsertManyAsync(sampleHabitos);
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
            }
        }

        public async Task<List<HabitoFitness>> GetHabitosAsync()
        {
<<<<<<< HEAD
            try 
            {
                return await _habitosCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo hábitos: {ex.Message}");
                return new List<HabitoFitness>();
            }
=======
            return await _habitosCollection.Find(_ => true).ToListAsync();
        }

        public async Task<HabitoFitness?> GetHabitoByIdAsync(string id)
        {
            return await _habitosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
        }

        public async Task CreateHabitoAsync(HabitoFitness habito)
        {
<<<<<<< HEAD
            try 
            {
                await _habitosCollection.InsertOneAsync(habito);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando hábito: {ex.Message}");
            }
=======
            await _habitosCollection.InsertOneAsync(habito);
        }

        public async Task UpdateHabitoAsync(string id, HabitoFitness habito)
        {
            await _habitosCollection.ReplaceOneAsync(x => x.Id == id, habito);
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
        }

        public async Task DeleteHabitoAsync(string id)
        {
<<<<<<< HEAD
            try 
            {
                await _habitosCollection.DeleteOneAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando hábito: {ex.Message}");
            }
=======
            await _habitosCollection.DeleteOneAsync(x => x.Id == id);
        }

        // Métodos específicos para análisis fitness
        public async Task<int> GetTotalCaloriasQuemadasAsync()
        {
            var habitos = await GetHabitosAsync();
            return habitos.Sum(h => h.CaloriasQuemadas);
        }

        public async Task<int> GetTotalMinutosEjercicioAsync()
        {
            var habitos = await GetHabitosAsync();
            return habitos.Sum(h => h.DuracionMinutos);
        }

        public async Task<Dictionary<string, int>> GetEstadisticasPorTipoAsync()
        {
            var habitos = await GetHabitosAsync();
            return habitos.GroupBy(h => h.Tipo)
                         .ToDictionary(g => g.Key, g => g.Count());
>>>>>>> b3dc4eb5a80c843991550a63cfe6cb986971b2e9
        }
    }
}
