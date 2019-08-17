using Autofac;
using Eklee.Azure.Functions.GraphQl;
using Eklee.Azure.Functions.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Example.InMemory.Core
{
	public class FunctionModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			// Use the built-in memory for caching.
			builder.UseDistributedCache<MemoryDistributedCache>();

			builder.RegisterGraphQl<SchemaConfig>();
			builder.RegisterType<QueryConfigObjectGraphType>();
			builder.RegisterType<MutationObjectGraphType>();
		}
	}
}
