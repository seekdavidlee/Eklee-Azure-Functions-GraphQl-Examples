using Example.DocumentDb.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.DocumentDb.Models
{
	public class IdInput : IIdEntity
	{
		[Key]
		[Description("Id of Input.")]
		public string Id { get; set; }
	}
}
