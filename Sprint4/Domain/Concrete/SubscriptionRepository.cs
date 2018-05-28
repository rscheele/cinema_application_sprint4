using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private EFDbContext context = new EFDbContext();

        public Subscription GetSubscription(int subscriptionId)
        {
            Subscription subscription = context.Subscriptions.Where(x => x.SubscriptionId == subscriptionId).FirstOrDefault();
            return subscription;
        }

        public void SaveSubscription(Subscription subscription)
        {
            context.Subscriptions.Add(subscription);
            context.SaveChanges();
        }
    }
}
