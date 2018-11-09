using System.Linq;
using App.Areas.Events.Models;
using GraphQL.Types;

namespace App.Areas.Events.Gql
{
    public class EventItemType : ObjectGraphType<Item>
    {
        public EventItemType()
        {
            Field(c => c.Id, type: typeof(IdGraphType));
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field(c => c.Name);
            Field(c => c.StartDate);
            Field(c => c.EndDate);
            Field(c => c.CreatorId, type: typeof(IdGraphType));
            Field(c => c.Details);
            Field<NonNullGraphType<ListGraphType<StringGraphType>>>("images",resolve: context => context.Source.Images.Select(it => it.ToString()));
        }
    }
}
