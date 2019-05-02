using App.Areas.Crm.Models;
using App.Areas.Crm.ViewModels;
using App.Areas.External.Models;
using AutoMapper;

namespace App.Areas.Crm.Utils 
{
    public class ExternalMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            // Identification
            config.CreateMap<GoogleAutoCompleteResult, AutoCompleteResult>();
            config.CreateMap<AutoCompleteResult, GoogleAutoCompleteResult>();
        }
    }
}
