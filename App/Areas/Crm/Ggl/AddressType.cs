using App.Areas.Crm.Models;
using GraphQL.Types;

namespace App.Areas.Crm.Ggl 
{
    public class AddressType : ObjectGraphType<Address>
    {
        public AddressType()
        {
            Field<StringGraphType>("id", resolve: c => c.Source.Id.ToString());
            Field(c => c.CreatedAt);
            Field(c => c.IsDeleted);
            Field<AddressCategoryEnum>("category", resolve: context => context.Source.Category);

            Field(c => c.LatLon, true);
            Field(c => c.OriginalFreeform);
            Field(c => c.StartDate, true);
            Field(c => c.EndDate, true);
            

            Field<CountryEnum>("country", resolve: context => context.Source.Country);
            Field(c => c.District, true);
            Field(c => c.County, true);
            Field(c => c.SubCounty, true);
        }
    }
}