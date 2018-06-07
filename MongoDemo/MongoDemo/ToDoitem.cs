using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDemo
{
    public class ToDoItem
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("todoItemName")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}
