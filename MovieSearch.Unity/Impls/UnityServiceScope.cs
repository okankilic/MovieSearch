using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Unity.Impls
{
    public class UnityServiceScope : IServiceScope
    {
        private readonly IUnityContainer _container;
        public IServiceProvider ServiceProvider { get; }

        public UnityServiceScope(IUnityContainer container)
        {
            _container = container;
            ServiceProvider = _container.Resolve<IServiceProvider>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
