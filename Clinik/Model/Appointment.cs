using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }

        public string? AppointmentNum { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }

        // Foreign key
        public int PatientID { get; set; }

        // Navigation property
        public virtual PatientModel Patient { get; set; }
    }
}
