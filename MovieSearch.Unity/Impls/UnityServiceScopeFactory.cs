using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Unity.Impls
{
    public class UnityServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IUnityContainer _container;

        public UnityServiceScopeFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IServiceScope CreateScope()
        {
            return new UnityServiceScope(CreateChildContainer());
        }

        private IUnityContainer CreateChildContainer()
        {
            var child = _container.CreateChildContainer();

            child.AddExtension(new EnumerableExtension());

            return child;
        }
    }
}
