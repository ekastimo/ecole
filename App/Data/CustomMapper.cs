using App.Areas.Chc.Utils;
using App.Areas.Crm.Utils;
using App.Areas.Doc.Utils;
using App.Areas.Events.Utils;
using AutoMapper;

namespace App.Data
{
    public class CustomMapper
    {
        public static void CreateConfigs(IMapperConfigurationExpression cfg)
        {
            ChcMapper.MapModels(cfg);
            EventsMapper.MapModels(cfg);
            DocumentsMapper.MapModels(cfg);
        }
    }
}
