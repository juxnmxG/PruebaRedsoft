using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PruebaRedsoft.Models
{
    [BsonIgnoreExtraElements]
    public class Automotive
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("plate")]
        public string Plate { get; set; }

        [BsonElement("model")]
        public string Model { get; set; }

        [BsonElement("is_ispected")]
        public bool IsInspected { get; set; }

    }
}
