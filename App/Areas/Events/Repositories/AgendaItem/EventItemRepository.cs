using App.Data;
using Core.Repositories;

namespace App.Areas.Events.Repositories.AgendaItem
{
    public class EventItemRepository : GenericRepository<Models.EventItem>, IEventItemRepository
    {
        public EventItemRepository(ApplicationDbContext context) : base(context)
        {
        }
 
       
    }
}
