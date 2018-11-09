using App.Areas.Doc.ViewModels;
using AutoMapper;

namespace App.Areas.Doc.Utils
{
    public class DocumentsMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            config.CreateMap<DocViewModel, Models.Doc>();
            config.CreateMap<Models.Doc, DocViewModel>();
        }
    }
}
