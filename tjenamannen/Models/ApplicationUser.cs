using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;

namespace tjenamannen.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; }
		public int? Kills { get; set; }
		public int? Deaths { get; set; }
		public List<Achievement>? Achievements { get; set; }
		//[ForeignKey("CompanyId")]
		//[ValidateNever]
		//public Company Company { get; set; }

	}
}
