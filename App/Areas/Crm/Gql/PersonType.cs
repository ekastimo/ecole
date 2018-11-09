using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Gql
{
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType()
        {
            Field(c => c.FirstName);
            Field(c => c.OtherNames);
            Field(c => c.DateOfBirth);
            Field(c => c.Avatar,true);
            Field(c => c.About,true);
            Field<NonNullGraphType<GenderEnum>>("gender", resolve: context => context.Source.Gender);
            Field<CivilStatusEnum>("civilStatus", resolve: context => context.Source.CivilStatus);
            Field<SalutationEnum>("salutation", resolve: context => context.Source.Salutation);
        }
    }
}
