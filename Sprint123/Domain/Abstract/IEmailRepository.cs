using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IEmailRepository
    {
        IEnumerable<EmailAdress> GetAllEmailAdresses();
        void SaveEmailAdress(EmailAdress emailAdress);
        EmailAdress FindEmailAdress(string emailAdress);
        void DeleteEmailAdress(string emailAdress);
    }
}
