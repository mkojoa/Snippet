using System.Data.Entity;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Repository
{
    public class StatusRepository : GenericRepository<Status, StatusDto>
    {
        public StatusRepository(DbContext context) : base(context)
        {
        }
    }
}