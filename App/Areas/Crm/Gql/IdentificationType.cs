using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Gql
{
    public class IdentificationType : ObjectGraphType<Identification>
    {
        public IdentificationType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field(c => c.IsPrimary);

            Field(c => c.Number);
            Field(c => c.IssuingCountry);
            Field(c => c.StartDate);
            Field(c => c.ExpiryDate);

            Field<IdentificationCategoryEnum>("category", resolve: context => context.Source.Category);
        }
    }
}
