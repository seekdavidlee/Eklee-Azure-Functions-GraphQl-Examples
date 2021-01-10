using GraphQL.Types;
using System;

namespace Example.Storage.Core
{
	public class SchemaConfig : Schema
	{
		public SchemaConfig(IServiceProvider resolver, QueryConfigObjectGraphType query, MutationObjectGraphType mutation) : base(resolver)
		{
			Query = query;
			Mutation = mutation;
		}
	}
}
