using System;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace MongoDemo
{
    public static class MongoService
    {
        static IMongoCollection<ToDoItem> todoItemsCollection;
        readonly static string dbName = "BlogDemo";
        readonly static string collectionName = "ToDoItems";
        static MongoClient client;

        static IMongoCollection<ToDoItem> ToDoItemsCollection
        {
            get
            {
                if (client == null || todoItemsCollection == null)
                {
                    var conx = "** ENTER YOUR CONNECTION STRING HERE **";
                    MongoClientSettings settings = MongoClientSettings.FromUrl(
                        new MongoUrl(conx)
                    );

                    settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };

                    client = new MongoClient(settings);
                    var db = client.GetDatabase(dbName);

                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    todoItemsCollection = db.GetCollection<ToDoItem>(collectionName, collectionSettings);
                }

                return todoItemsCollection;
            }
        }

        public async static Task<List<ToDoItem>> GetAllItems()
        {
            var allItems = await ToDoItemsCollection
                .Find(new BsonDocument())
                .ToListAsync();

            return allItems;
        }

        public async static Task<List<ToDoItem>> SearchByName(string name)
        {
            var results = await ToDoItemsCollection
                            .AsQueryable()
                            .Where(tdi => tdi.Name.Contains(name))
                            .Take(10)
                            .ToListAsync();

            return results;
        }

        public async static Task InsertItem(ToDoItem item)
        {
            await ToDoItemsCollection.InsertOneAsync(item);
        }

        public async static Task<bool> DeleteItem(ToDoItem item)
        {
            var result = await ToDoItemsCollection.DeleteOneAsync(tdi => tdi.Id == item.Id);

            return result.IsAcknowledged && result.DeletedCount == 1;
        }
    }
}
