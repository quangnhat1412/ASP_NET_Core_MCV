using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Demo_Etity_Frameword_Core.Models
{
    public class Category
    {
        [Key] // xac dinh khoa chinh
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên thể loại !")] // khong dc bo trong
        public String Name { get; set; }

        [Range(1,100,ErrorMessage = "Vui lòng nhập từ 1 -> 100 !")] 
        public int DisplayOrder { get; set; }
    }
}
