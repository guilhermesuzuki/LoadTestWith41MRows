using FluentNHibernate.Mapping;
using NHibernate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Mappings
{
    internal class ProfessionMap: ClassMap<Profession>
    {
        public ProfessionMap()
        {
            Id(x => x.Id).Unique().GeneratedBy.Identity();
            Map(x => x.NConst).Length(25);
            Map(x => x.Description).Length(50);
            Map(x => x.ColumnForUpdateTest).Length(50);
            Table("Professions");
        }
    }
}
