using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class SalesTempTicketModel
    {
        public List<TempTicket> TempTickets;

        public decimal TotalPrice;

        public decimal CurrentPrice;

        public List<Deduction> Deductions;

    }

    public class Deduction
    {
        private long TicketId { get; set; }
        private string Name { get; set; }
        private decimal Amount { get; set; }
    }
}