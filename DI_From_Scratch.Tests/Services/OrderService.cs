using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests.Services
{
    public class PaymentService
    {
        public string Pay() => "Payment processed";
    }

    // A class that depends on PaymentService
    public class OrderService
    {
        public PaymentService PaymentService { get; }
        public OrderService(PaymentService paymentService)
        {
            PaymentService = paymentService;
        }
    }
}
