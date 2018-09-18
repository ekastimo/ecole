using App.Areas.Crm.ViewModels;
using GraphQL.Types;

namespace App.Areas.Crm.Ggl
{
    public class ContactSearchType : InputObjectGraphType<ContactSearchRequest>
    {
        public ContactSearchType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.Query, true);
            Field(c => c.Skip, true);
            Field(c => c.Limit, true);
            Field(c => c.Name, true);
            Field(c => c.Email, true);
            Field(c => c.Phone, true);
        }
    }
}