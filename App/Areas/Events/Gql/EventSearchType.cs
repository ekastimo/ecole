using App.Areas.Events.ViewModels;
using GraphQL.Types;

namespace App.Areas.Events.Gql
{
    public class EventSearchType : InputObjectGraphType<EventSearchRequest>
    {
        public EventSearchType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.Query, true);
            Field(c => c.Skip, true);
            Field(c => c.Limit, true);
            Field(c => c.ContactId, true, typeof(IdGraphType));
        }
    }
}