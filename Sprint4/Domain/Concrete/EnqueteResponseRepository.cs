using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EnqueteResponseRepository : IEnqueteResponseRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<EnqueteResponse> GetAllEnqueteResponses()
        {
            return context.EnqueteResponse;
        }

        public void SaveEnqueteEnqueteResponse(EnqueteResponse enqueteResponse)
        {
            context.EnqueteResponse.Add(enqueteResponse);
            context.SaveChanges();
        }

        public EnqueteResponse FindEnqueteResponse(string ticketCode)
        {
            EnqueteResponse dbEntry = context.EnqueteResponse.Where(x => x.TicketCode == ticketCode).FirstOrDefault();
            return dbEntry;
        }

    }
}
