using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Ggl
{
    public class CompanyType : ObjectGraphType<Company>
    {
        public CompanyType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);

            Field(c => c.Name);
            Field<CompanyCategoryEnum>("category", resolve: context => context.Source.Category);
        }
    }
}
