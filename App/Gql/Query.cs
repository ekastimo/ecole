using System;
using App.Areas.Crm.Gql;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.ViewModels;
using App.Areas.Doc.Gql;
using App.Areas.Doc.Repositories;
using App.Areas.Doc.ViewModels;
using App.Areas.Events.Gql;
using App.Areas.Events.Repositories.Event;
using App.Areas.Events.ViewModels;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace App.Gql
{
    public class Query : ObjectGraphType<object>
    {
        public Query(IServiceProvider serviceProvider)
        {
            Name = "Query";

            Field<ListGraphType<ContactType>>(
                "contacts",
                arguments: new QueryArguments(
                    new QueryArgument<ContactSearchType> {Name = "args"}
                ),
                resolve: context =>
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
                        var args = context.GetArgument<ContactSearchRequest>("args")?? new ContactSearchRequest();
                        return repository.SearchAsync(args,true).GetAwaiter().GetResult();
                    }
                }
            );

            Field<ListGraphType<EventType>>(
                "events",
                arguments: new QueryArguments(
                    new QueryArgument<EventSearchType> { Name = "args" }
                ),
                resolve: context =>
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
                        var args = context.GetArgument<EventSearchRequest>("args") ?? new EventSearchRequest();
                        return repository.SearchAsync(args, true).GetAwaiter().GetResult();
                    }
                }
            );

            Field<ListGraphType<DocType>>(
                "documents",
                arguments: new QueryArguments(
                    new QueryArgument<DocSearchType> { Name = "args" }
                ),
                resolve: context =>
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<IDocRepository>();
                        var args = context.GetArgument<DocSearchRequest>("args") ?? new DocSearchRequest();
                        return repository.SearchAsync(args, true).GetAwaiter().GetResult();
                    }
                }
            );
        }
    }
}