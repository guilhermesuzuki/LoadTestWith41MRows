using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Models
{
    public class Title
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string NConst { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public virtual string ColumnForUpdateTest { get; set; } = string.Empty;
    }
}
