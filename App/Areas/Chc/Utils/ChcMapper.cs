using App.Areas.Chc.Models;
using App.Areas.Chc.ViewModel;
using App.Areas.Crm.Models;
using App.Areas.Crm.ViewModels;
using AutoMapper;

namespace App.Areas.Crm.Utils 
{
    public class ChcMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            // Location
            config.CreateMap<LocationViewModel, Location>();
            config.CreateMap<Location, LocationViewModel>();

            // CCellGroup
            config.CreateMap<CellGroupViewModel, CellGroup>();
            config.CreateMap<CellGroup, CellGroupViewModel>();

        }
    }
}
