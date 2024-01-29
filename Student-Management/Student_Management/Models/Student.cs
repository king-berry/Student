using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Management.Models
{
	public class Student
	{
		[Key]
		public string Id { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
        public bool Status { get; set; }
        public string? Comment { get; set; }
        public int ClassId { get; set; }
		public Class Class { get; set; }
	}
}
