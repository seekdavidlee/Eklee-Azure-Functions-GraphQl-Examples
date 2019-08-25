using GraphQL;
using GraphQL.Types;

namespace Example.Storage.Core
{
	public class SchemaConfig : Schema
	{
		public SchemaConfig(IDependencyResolver resolver, QueryConfigObjectGraphType query, MutationObjectGraphType mutation) : base(resolver)
		{
			Query = query;
			Mutation = mutation;
		}
	}
}
