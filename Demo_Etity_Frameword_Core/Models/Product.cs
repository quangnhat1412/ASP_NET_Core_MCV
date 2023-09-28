using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo_Etity_Frameword_Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description  { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int CategoryID { get; set; }
        // khai bao moi ket hop 1-n
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
