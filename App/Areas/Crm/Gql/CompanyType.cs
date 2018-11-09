using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Gql
{
    public class CompanyType : ObjectGraphType<Company>
    {
        public CompanyType()
        {
            Field(c => c.Name);
            Field<CompanyCategoryEnum>("category", resolve: context => context.Source.Category);
        }
    }
}
