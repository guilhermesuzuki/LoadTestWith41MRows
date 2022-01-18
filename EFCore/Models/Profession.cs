using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Models
{
    [Table("Professions")]
    public class Profession
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string NConst { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        [Column]
        public string ColumnForUpdateTest { get; set; } = string.Empty;
    }
}
