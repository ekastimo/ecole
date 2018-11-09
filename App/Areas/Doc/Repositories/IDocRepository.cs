using System.Collections.Generic;
using System.Threading.Tasks;
using App.Areas.Doc.ViewModels;
using Core.Repositories;

namespace App.Areas.Doc.Repositories
{
    public interface IDocRepository : IGenericRepository<Models.Doc>
    {
        Task<IEnumerable<Models.Doc>> SearchAsync(DocSearchRequest request, bool fullQuery = false);
    }
}
