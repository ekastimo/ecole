using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Ggl
{
    public class EmailType : ObjectGraphType<Email>
    {
        public EmailType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field(c => c.IsPrimary);
            Field(c => c.Address);
            Field<EmailCategoryEnum>("category", resolve: context => context.Source.Category);
        }
    }
}
