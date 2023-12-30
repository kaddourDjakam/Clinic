using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinik.Model
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        // Navigation property
        public virtual ICollection<Treatment> Treatments { get; set; } = new HashSet<Treatment>();
    }
}
