﻿using LukurareBackend.Models;
using LukurareBackend.Repositories.Accounts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectBaseWeb.Controllers.Repository;
using System.Threading.Tasks;

namespace Lukurare.Controllers.Accounts
{

    [Route("Accounts/Payments")]
    public class PaymentsController
        : BulkSMSGatewayBaseGridController<Payment, PaymentRepository>
    {

        private readonly IWebHostEnvironment _hostingEnvironment;


        public PaymentsController(IWebHostEnvironment hostingEnvironment)
            : base(new PaymentRepository("Payment"))
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
