using System.Threading.Tasks;
using App.Areas.Doc.ViewModels;

namespace App.Areas.Doc.Services
{
    public interface IDocService
    {
        Task<DocViewModel> Upload(UploadRequest request);
    }
}