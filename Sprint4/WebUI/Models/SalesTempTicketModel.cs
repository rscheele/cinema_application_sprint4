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
        public int Id { get; set; }
        public long TicketId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}