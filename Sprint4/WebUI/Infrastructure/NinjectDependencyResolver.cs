using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Ninject;

namespace Sprint123.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IShowRepository>().To<ShowRepository>();
            kernel.Bind<ITicketRepository>().To<TicketRepository>();
            kernel.Bind<IMovieOverviewRepository>().To<MovieOverviewRepository>();
            kernel.Bind<ITempTicketRepository>().To<TempTicketRepository>();
            kernel.Bind<IShowSeatRepository>().To<ShowSeatRepository>();
            kernel.Bind<IRoomRepository>().To<RoomRepository>();
            kernel.Bind<IEmailRepository>().To<EmailRepository>();
            kernel.Bind<IEnqueteResponseRepository>().To<EnqueteResponseRepository>();
            kernel.Bind<ISubscriptionRepository>().To<SubscriptionRepository>();

        }
    }
}