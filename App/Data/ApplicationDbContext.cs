using System.Diagnostics;
using App.Areas.Crm.Models;
using App.Areas.Doc.Models;
using App.Areas.Events.Models;
using Core.Extensions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace App.Data
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IConfiguration config)
        {
            var mongoConnectionUrl = new MongoUrl(config.GetMongoConnection());
            var settings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            settings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    var msg = $"{e.CommandName} - {e.Command.ToJson()}";
                    Debug.WriteLine(msg);
                });
            };
            ;
            var client = new MongoClient(settings);
            _database = client.GetDatabase(config.GetMongoDatabase());
        }

        public IMongoCollection<Contact> Contacts => _database.GetCollection<Contact>("contacts");
        public IMongoCollection<Event> Events => _database.GetCollection<Event>("events");
        public IMongoCollection<Todo> Todos => _database.GetCollection<Todo>("todos");
        public IMongoCollection<Doc> Docs => _database.GetCollection<Doc>("docs");
    }
}