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
        [ForeignKey("Person")]
        public int ID { get; set; }

        [StringLength(50)] // Adjust the maximum length as needed
        public string? Barcode { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public Gender Gender { get; set; }

        // ... other properties

        // Navigation property
        public virtual Person Person { get; set; }

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
