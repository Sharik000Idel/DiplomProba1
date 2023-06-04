using System.ComponentModel;

namespace DiplomProba1.Models
{
	public class TestImagecs
	{
		[DisplayName("Upload Image")]
		public string? NameImage { get; set; }
		public IFormFile? UserImg { get; set; }
	}
}
