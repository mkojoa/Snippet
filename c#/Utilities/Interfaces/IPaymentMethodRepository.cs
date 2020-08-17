using System.Collections.Generic;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;

namespace eticketing_mvc.Utilities.Interfaces
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod, PaymentMethodDto>
    {
        IEnumerable<PaymentMethodDto> GetPaymentMethods();
    }
}
