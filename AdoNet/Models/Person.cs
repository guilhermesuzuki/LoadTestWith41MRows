using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNet.Models
{
    public class Person
    {
        /// <summary>
        /// alphanumeric unique identifier of the name/person
        /// </summary>
        public virtual string NConst { get; set; } = string.Empty;
        /// <summary>
        /// name by which the person is most often credited
        /// </summary>
        public virtual string PrimaryName { get; set; } = string.Empty;
        /// <summary>
        /// in YYYY format
        /// </summary>
        public virtual short? BirthYear { get; set; }
        /// <summary>
        /// in YYYY format if applicable, else '\N'
        /// </summary>
        public virtual short? DeathYear { get; set; }
        /// <summary>
        /// (array of strings)– the top-3 professions of the person
        /// </summary>
        public virtual ICollection<Profession> PrimaryProfession { get; set; } = new List<Profession>();
        /// <summary>
        /// (array of tconsts) – titles the person is known for
        /// </summary>
        public virtual ICollection<Title> KnownForTitles { get; set; } = new List<Title>();
        /// <summary>
        /// Column created especially for Updates
        /// </summary>
        public virtual string ColumnForUpdateTest { get; set; } = string.Empty;
    }
}
