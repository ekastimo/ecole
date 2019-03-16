using System.Diagnostics;
using App.Areas.Chc.Models;
using App.Areas.Crm.Models;
using App.Areas.Doc.Models;
using App.Areas.Events.Models;
using App.Areas.Teams.Models;
using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace App.Data
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IConfiguration config,ILogger<ApplicationDbContext> logger)
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
            var client = new MongoClient(settings);
            _database = client.GetDatabase(config.GetMongoDatabase());
            var index= Builders<TeamMember>.IndexKeys.Ascending(d => d.ContactId).Descending(d => d.TeamId);
            var options = new CreateIndexOptions { Unique = true,Sparse=true };
            var indexModel = new CreateIndexModel<TeamMember>(index, options);
            var resp = TeamMembers.Indexes.CreateOne(indexModel);
            logger.LogInformation($"created index on team members {resp}");
        }

        public IMongoCollection<Contact> Contacts => _database.GetCollection<Contact>("Contacts");
        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<Todo> Todos => _database.GetCollection<Todo>("Todos");
        public IMongoCollection<Doc> Docs => _database.GetCollection<Doc>("Docs");
        public IMongoCollection<Team> Teams => _database.GetCollection<Team>("Teams");
        public IMongoCollection<TeamMember> TeamMembers => _database.GetCollection<TeamMember>("TeamMembers");

        public IMongoCollection<Location> Locations => _database.GetCollection<Location>("Locations");
        public IMongoCollection<CellGroup> CellGroups => _database.GetCollection<CellGroup>("CellGroups");

    }
}