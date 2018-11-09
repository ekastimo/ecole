using App.Areas.Events.ViewModels;
using GraphQL.Types;

namespace App.Areas.Doc.Gql
{
    public class DocSearchType : InputObjectGraphType<EventSearchRequest>
    {
        public DocSearchType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.Query, true);
            Field(c => c.Skip, true);
            Field(c => c.Limit, true);
            Field(c => c.ContactId, true, typeof(IdGraphType));
        }
    }
}