using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using eticketing_mvc.ModelDTOs;
using eticketing_mvc.Models;
using eticketing_mvc.Utilities.Interfaces;

namespace eticketing_mvc.Utilities.Repository
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod, PaymentMethodDto>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(DbContext context) : base(context)
        {
            
        }

        public IEnumerable<PaymentMethodDto> GetPaymentMethods()
        {
            var payment = new List<PaymentMethodDto>
            {
                new PaymentMethodDto
                {
                    PaymentMethodName = "Credit Card",
                    PaymentMethodId = 1
                },
                new PaymentMethodDto
                {
                    PaymentMethodName = "Bank",
                    PaymentMethodId = 2
                },
                new PaymentMethodDto
                {
                    PaymentMethodName = "Mobile Money",
                    PaymentMethodId = 3
                },
                new PaymentMethodDto
                {
                    PaymentMethodName = "Pay Later",
                    PaymentMethodId = 4
                }
            };
            return payment;
        }
    }
}