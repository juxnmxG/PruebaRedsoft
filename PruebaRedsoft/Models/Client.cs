using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PruebaRedsoft.Models
{
    [BsonIgnoreExtraElements]
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("dni")]
        public string Dni { get; set; }

        [BsonElement("bith_date")]
        public DateTime BirthDate { get; set; }

        [BsonElement("city_of_residence")]
        public string CityOfResidence { get; set; }

        [BsonElement("Addres_of_residence")]
        public string AddresOfResidence { get; set; }
    }
}
