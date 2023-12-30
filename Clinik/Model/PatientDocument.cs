using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class PatientDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

        public byte[]? DocImage { get; set; } // Assuming the document image is stored as a byte array
        public int PatientID { get; set; }
        public string? Description { get; set; }

        // Navigation property
        public virtual PatientModel? Patient { get; set; }
    }
}
