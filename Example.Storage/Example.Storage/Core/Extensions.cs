using Eklee.Azure.Functions.GraphQl;
using Example.Storage.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq.Expressions;

namespace Example.Storage.Core
{
	public static class Extensions
	{
		public static void Add<T, TProperty>(
			this MutationObjectGraphType spmMutation,
			InputBuilderFactory inputBuilderFactory,
			IConfiguration configuration,
			Expression<Func<T, TProperty>> expression) where T : class, IIdEntity, new()
		{
			inputBuilderFactory.Create<T>(spmMutation)
				.ConfigureTableStorage<T>()
				.AddConnectionString(configuration["Storage:ConnectionString"])
				.AddPartition(expression)
				.BuildTableStorage()
				.Delete<IdInput, Status>(m => new T { Id = m.Id }, t => new Status())
				.DeleteAll(() => new Status { Message = "All entities removed." })
				.Build();
		}
	}
}
