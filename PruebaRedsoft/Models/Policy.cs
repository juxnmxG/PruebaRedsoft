using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PruebaRedsoft.Models
{
    [BsonIgnoreExtraElements]
    public class Policy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("number_policy")]
        public int NumberPolicy { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("coverages")]
        public List<string> Coverages { get; set; }

        [BsonElement("value_max")]
        public decimal ValueMax { get; set; }

        [BsonElement("plan")]
        public string Plan { get; set; }

        [BsonElement("date_init")]
        public DateTime DateInit { get; set; }

        [BsonElement("date_End")]
        public DateTime DateEnd { get; set; }

        [BsonElement("client_policy")]
        public Client ClientPolicy { get; set; }

        [BsonElement("automotive_policy")]
        public Automotive AutomotivePolicy { get; set; }

    }
}
