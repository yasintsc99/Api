using MongoDB.Bson.Serialization.Attributes;

namespace Api.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("CategoryID")]
        public int? CategoryID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
