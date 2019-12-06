using App.Areas.Chc.Models;
using App.Areas.Crm.Models;
using App.Areas.Doc.Models;
using App.Areas.Events.Models;
using App.Areas.Groups.Models;
using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace App.Data
{
    public static class ClientHolder
    {
        private static MongoClient Client;

        public static MongoClient GetClient(MongoClientSettings settings)
        {
            return Client ?? (Client = new MongoClient(settings));
        }
    }

    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IConfiguration config, ILogger<ApplicationDbContext> logger)
        {
            var mongoConnectionUrl = new MongoUrl(config.GetMongoConnection());
            var settings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            settings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    var msg = $"{e.CommandName} - {e.Command.ToJson()}";
                    logger.LogInformation(msg);
                });
            };
            var client = ClientHolder.GetClient(settings);
            _database = client.GetDatabase(config.GetMongoDatabase());
            var index = Builders<Member>.IndexKeys.Ascending(d => d.ContactId).Descending(d => d.GroupId);
            var options = new CreateIndexOptions {Unique = true, Sparse = true};
            var indexModel = new CreateIndexModel<Member>(index, options);
            var resp = GroupMembers.Indexes.CreateOne(indexModel);
            logger.LogInformation($"created index on team members {resp}");
        }

        public IMongoCollection<Contact> Contacts => _database.GetCollection<Contact>("Contacts");
        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<Todo> Todos => _database.GetCollection<Todo>("Todos");
        public IMongoCollection<Doc> Docs => _database.GetCollection<Doc>("Docs");
        public IMongoCollection<Areas.Tags.Tag> Tags => _database.GetCollection<Areas.Tags.Tag>("Tags");
        public IMongoCollection<Group> Groups => _database.GetCollection<Group>("Groups");
        public IMongoCollection<Member> GroupMembers => _database.GetCollection<Member>("GroupMembers");

        public IMongoCollection<Location> Locations => _database.GetCollection<Location>("Locations");
        public IMongoCollection<CellGroup> CellGroups => _database.GetCollection<CellGroup>("CellGroups");
    }
}