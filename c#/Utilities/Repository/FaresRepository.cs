using System.Data.Entity;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class FaresRepository : GenericRepository<Fare, FareDto>, IFaresRepository
    {
        public FaresRepository(DbContext context) : base(context)
        {
            
        }
    }
}