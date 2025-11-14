using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBReports.Models
{
    public class HabitoFitness
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Actividad { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // Cardio, Fuerza, Flexibilidad, etc.
        public int DuracionMinutos { get; set; } // Duración en minutos
        public int CaloriasQuemadas { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Intensidad { get; set; } = "Moderada"; // Baja, Moderada, Alta
        public string Notas { get; set; } = string.Empty;
    }
}
