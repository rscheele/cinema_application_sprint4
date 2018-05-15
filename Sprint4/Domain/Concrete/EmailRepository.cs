using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EmailRepository : IEmailRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<EmailAdress> GetAllEmailAdresses()
        {
            return context.EmailAdresses;
        }

        public void SaveEmailAdress(EmailAdress emailAdress)
        {
            EmailAdress dbEntry = context.EmailAdresses.Where(x => x.Email == emailAdress.Email).FirstOrDefault();
            if (dbEntry == null)
            {
                context.EmailAdresses.Add(emailAdress);
                context.SaveChanges();
            }
        }

        public EmailAdress FindEmailAdress(string emailAdress)
        {
            EmailAdress dbEntry = context.EmailAdresses.Where(x => x.Email == emailAdress).FirstOrDefault();
            return dbEntry;
        }

        public void DeleteEmailAdress(string emailAdress)
        {
            EmailAdress dbEntry = context.EmailAdresses.Where(x => x.Email == emailAdress).FirstOrDefault();
            context.EmailAdresses.Remove(dbEntry);
            context.SaveChanges();
        }
    }
}
