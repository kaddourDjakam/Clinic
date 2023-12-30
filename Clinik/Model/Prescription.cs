using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionID { get; set; }

        // Foreign key
        public int TreatmentID { get; set; }

        // Assuming the prescription image is stored as a byte array
        public byte[]? PrescriptionImage { get; set; }

        // Navigation property
        public virtual Treatment? Treatment { get; set; }
    }
}
