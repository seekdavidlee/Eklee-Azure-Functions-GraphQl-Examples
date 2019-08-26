using Eklee.Azure.Functions.GraphQl;
using Example.DocumentDb.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq.Expressions;

namespace Example.DocumentDb.Core
{
	public static class Extensions
	{
		public static void Add<T, TProperty>(
			this MutationObjectGraphType mutation,
			InputBuilderFactory inputBuilderFactory,
			IConfiguration configuration,
			Expression<Func<T, TProperty>> expression) where T : class, IIdEntity, new()
		{
			inputBuilderFactory.Create<T>(mutation)
				.ConfigureDocumentDb<T>()
				.AddDatabase(configuration["Db:Name"])
				.AddPartition(expression)
				.AddRequestUnit(Convert.ToInt32(configuration["Db:RequestUnits"]))
				.AddKey(configuration["Db:Key"])
				.AddUrl(configuration["Db:Url"])
				.BuildDocumentDb()
				.Delete<IdInput, Status>(m => new T { Id = m.Id }, t => new Status())
				.DeleteAll(() => new Status { Message = "All entities removed." })
				.Build();
		}
	}
}
