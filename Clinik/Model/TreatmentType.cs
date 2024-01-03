using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class TreatmentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TreatmentTypeID { get; set; }

        public string? TreatmentName { get; set; }

        // Navigation properties
        public virtual ICollection<Treatment> Treatments { get; set; } 
            =
            new HashSet<Treatment>();
    }
}
