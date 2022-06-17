using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyOn.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string PhotoPath { get; set; }
        public string PrescriptionPhotoPath { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string TotalPrice { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public Boolean Prescription{get;set;}
    }
}
