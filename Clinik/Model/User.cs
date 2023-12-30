using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }


        // ... other properties

        // Navigation properties
        public virtual ICollection<Person> Persons { get; set; } = new HashSet<Person>();
    }

}
