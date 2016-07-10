using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using WeatherWebApp.Models.Logger;

namespace WeatherWebApp.Container
{
    public class Ioc : IDependencyResolver
    {
        private readonly IKernel _kernel;
        public Ioc(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            _kernel.Bind<ILogger>().To<DebugConsoleLogger>().InSingletonScope();

        }
    }
}
