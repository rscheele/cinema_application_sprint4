using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface ISubscriptionRepository
    {
        void SaveSubscription(Subscription subscription);
        Subscription GetSubscription(int subscriptionId);
    }
}
