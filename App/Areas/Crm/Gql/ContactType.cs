using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Gql
{
    public class ContactType : ObjectGraphType<Contact>
    {
        public ContactType()
        {
            Field(c => c.Id, type: typeof(IdGraphType));
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field<ContactCategoryEnum>("category", resolve: context => context.Source.Category);
            Field<PersonType>("person", resolve: c => c.Source.Person);
            Field<CompanyType>("company", resolve: c => c.Source.Company);

            Field<NonNullGraphType<ListGraphType<EmailType>>>("emails", resolve: c => c.Source.Emails);
            Field<NonNullGraphType<ListGraphType<PhoneType>>>("phones", resolve: c => c.Source.Phones);

            Field<ListGraphType<AddressType>>("addresses", resolve: c => c.Source.Addresses);
            Field<ListGraphType<IdentificationType>>("identifications", resolve: c => c.Source.Identifications);
            Field<ListGraphType<StringGraphType>>("tags", resolve: c => c.Source.Tags);
        }
    }
}