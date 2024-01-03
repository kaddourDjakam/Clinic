using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class Person
    {
        public Person()
        {
            // Note: One-to-one relationship, so a Person can have at most one PatientModel
            this.Patient = new PatientModel();
            this.Users = new HashSet<User>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)] // Adjust the maximum length as needed
        public string Fullname { get; set; }

        [StringLength(20)] // Adjust the maximum length as needed
        public string? Phone { get; set; }

        [StringLength(255)] // Adjust the maximum length as needed
        public string? Adress { get; set; }

        // ... other properties

        // Navigation property
        public virtual PatientModel Patient { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

}
