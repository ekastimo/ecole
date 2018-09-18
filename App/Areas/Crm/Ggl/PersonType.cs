using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Ggl
{
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field(c => c.FirstName);
            Field(c => c.OtherNames);
            Field(c => c.About,true);
            Field<NonNullGraphType<GenderEnum>>("gender", resolve: context => context.Source.Gender);
            Field<CivilStatusEnum>("civilStatus", resolve: context => context.Source.CivilStatus);
            Field<SalutationEnum>("salutation", resolve: context => context.Source.Salutation);
        }
    }
}
