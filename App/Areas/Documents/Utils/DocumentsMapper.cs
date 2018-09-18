using App.Areas.Documents.Models;
using App.Areas.Documents.ViewModels;
using AutoMapper;

namespace App.Areas.Documents.Utils
{
    public class DocumentsMapper
    {
        public static void MapModels(IMapperConfigurationExpression config)
        {
            config.CreateMap<DocumentViewModel, Document>();
            config.CreateMap<Document, DocumentViewModel>();
        }
    }
}
