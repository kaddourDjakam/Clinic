using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class PatientModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Barcode { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; } // Assuming Gender is an enum

        // ... other properties

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public virtual ICollection<PatientDocument> Documents { get; set; } = new HashSet<PatientDocument>();
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
