using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Management.Models
{
	public class Class
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Student>? Students { get; set; }
		
    }
}