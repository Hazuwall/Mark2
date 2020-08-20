using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public interface IPluginStartup
    {
        int Order { get; }

        void ConfigureServices(IServiceCollection services);
        void Configure(IOperationPipelineBuilder pipeline, IContractRegistry contracts, IServiceProvider services);
    }
}
