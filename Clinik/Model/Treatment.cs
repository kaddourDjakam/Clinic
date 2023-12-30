using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class Treatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TreatmentID { get; set; }

        public decimal Price { get; set; }

        // Foreign keys
        public int PaymentID { get; set; }
        public int AppointmentID { get; set; }

        // Navigation properties
        public virtual Payment? Payment { get; set; }
        public virtual Appointment? Appointment { get; set; }

        // Many-to-many relationship with TreatmentType
        public virtual ICollection<TreatmentType> TreatmentTypes { get; set; } = new HashSet<TreatmentType>();

        // Navigation property for prescriptions
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
    }
}
