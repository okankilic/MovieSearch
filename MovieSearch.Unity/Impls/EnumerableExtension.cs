using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Unity.Impls
{
    public class EnumerableExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            // Enumerable strategy
            Context.Strategies.AddNew<EnumerableResolutionStrategy>(
                UnityBuildStage.TypeMapping);
        }
    }
}
