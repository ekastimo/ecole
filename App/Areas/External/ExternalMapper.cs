using App.Areas.External.Models;
using AutoMapper;

namespace App.Areas.External 
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
