using Microsoft.Extensions.DependencyInjection;
using System;

namespace MuffiNet.FrontendReact.DomainModel
{
    public interface IDomainModelBuilder
    {
        IServiceCollection Services { get; }
    }

    public class DomainModelBuilder : IDomainModelBuilder
    {
        public IServiceCollection Services { get; }

        public DomainModelBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}