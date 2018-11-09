using App.Areas.Events.Models;
using App.Areas.Events.ViewModels;
using AutoMapper;

namespace App.Areas.Events.Utils
{
    public class EventsMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
      
            config.CreateMap<EventViewModel, Event>();
            config.CreateMap<Event, EventViewModel>();

            config.CreateMap<ItemViewModel, Item>();
            config.CreateMap<Item, ItemViewModel>();

        }
    }
}
