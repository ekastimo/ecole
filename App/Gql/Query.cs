using System;
using App.Areas.Crm.Ggl;
using App.Areas.Crm.Repositories.Contact;
using App.Areas.Crm.ViewModels;
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
                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        var contactRepository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
                        var args = context.GetArgument<ContactSearchRequest>("args")?? new ContactSearchRequest();
                        return contactRepository.SearchAsync(args,true).GetAwaiter().GetResult();
                    }
                }
            );

            Field<ListGraphType<ContactType>>(
                "events",
                arguments: new QueryArguments(
                    new QueryArgument<ContactSearchType> { Name = "args" }
                ),
                resolve: context =>
                {
                    using (IServiceScope scope = serviceProvider.CreateScope())
                    {
                        var contactRepository = scope.ServiceProvider.GetRequiredService<IContactRepository>();
                        var args = context.GetArgument<ContactSearchRequest>("args") ?? new ContactSearchRequest();
                        return contactRepository.SearchAsync(args, true).GetAwaiter().GetResult();
                    }
                }
            );
        }
    }
}