using App.Areas.Crm.Ggl;
using App.Areas.Crm.Models;
using App.Areas.Events.Models;
using GraphQL.Types;

namespace App.Areas.Events.Gql
{
    public class EventType : ObjectGraphType<Event>
    {
        public EventType()
        {
            Field(c => c.Id, type: typeof(IdGraphType));
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);

            Field(c => c.StartDate);
            Field(c => c.EndDate);
            Field(c => c.CreatorId);
        }
    }
}