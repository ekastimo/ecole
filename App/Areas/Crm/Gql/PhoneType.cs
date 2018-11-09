using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Gql
{
    public class PhoneType : ObjectGraphType<Phone>
    {
        public PhoneType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field(c => c.IsPrimary);
            Field(c => c.Number);
            Field<PhoneCategoryEnum>("category", resolve: context => context.Source.Category);
        }
    }
}
