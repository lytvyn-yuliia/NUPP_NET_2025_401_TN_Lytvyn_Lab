using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Journalism.NoSql.Models
{
    public class ArticleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("journalist")]
        public string Journalist { get; set; }
    }
}
