using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore7.Models
{
    [Table("People")]
    public class Person
    {
        /// <summary>
        /// alphanumeric unique identifier of the name/person
        /// </summary>
        [Key]
        public string NConst { get; set; } = string.Empty;
        /// <summary>
        /// name by which the person is most often credited
        /// </summary>
        [Column]
        public string PrimaryName { get; set; } = string.Empty;
        /// <summary>
        /// in YYYY format
        /// </summary>
        [Column]
        public short? BirthYear { get; set; }
        /// <summary>
        /// in YYYY format if applicable, else '\N'
        /// </summary>
        [Column]
        public short? DeathYear { get; set; }
        /// <summary>
        /// (array of strings)– the top-3 professions of the person
        /// </summary>
        [Column]
        public List<Profession> PrimaryProfession { get; set; } = new List<Profession>();
        /// <summary>
        /// (array of tconsts) – titles the person is known for
        /// </summary>
        public List<Title> KnownForTitles { get; set; } = new List<Title>();
        /// <summary>
        /// Column created especially for Updates
        /// </summary>
        [Column]
        public string ColumnForUpdateTest { get; set; } = string.Empty;
    }
}
