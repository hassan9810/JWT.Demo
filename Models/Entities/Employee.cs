using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT.Demo.Models.Entities
{
    public class Employee
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
