using DI_From_Scratch.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_From_Scratch.Tests.Services
{
    public interface IOrderService { }
    public interface IPaymentService { }

    [Transient]
    public class PaymentService : IPaymentService
    {
        public string Pay() => "Payment processed";
    }

    // A class that depends on PaymentService
    [Transient]
    public class OrderService : IOrderService
    {
        public PaymentService PaymentService { get; }
        public OrderService(PaymentService paymentService)
        {
            PaymentService = paymentService;
        }
    }
}
