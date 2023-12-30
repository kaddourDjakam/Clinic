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
            this.Patients = new HashSet<PatientModel>();
            this.Users = new HashSet<User>();
        }

        [Key]
        public int ID { get; set; }
        public string Fullname { get; set; }
        public string? Phone { get; set; }
        public string? Adress { get; set; }

        // ... other properties

        public virtual ICollection<PatientModel>? Patients { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
