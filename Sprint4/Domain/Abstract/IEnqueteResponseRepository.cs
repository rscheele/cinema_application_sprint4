using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IEnqueteResponseRepository
    {
        IEnumerable<EnqueteResponse> GetAllEnqueteResponses();
        void SaveEnqueteEnqueteResponse(EnqueteResponse enqueteResponse);
        EnqueteResponse FindEnqueteResponse(string ticketCode);

    }
}
