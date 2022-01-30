using FluentNHibernate.Mapping;
using NHibernate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Mappings
{
    internal class PersonMap: ClassMap<Person>
    {
        public PersonMap(): base()
        {
            DynamicInsert();
            DynamicUpdate();

            Id(x => x.NConst);
            Map(x => x.PrimaryName).Length(250);
            Map(x => x.BirthYear).Nullable();
            Map(x => x.DeathYear).Nullable();
            Map(x => x.ColumnForUpdateTest).Nullable();
            HasMany(x => x.PrimaryProfession).Cascade.None().LazyLoad().KeyColumn("NConst");
            HasMany(x => x.KnownForTitles).Cascade.None().LazyLoad().KeyColumn("NConst");
            Table("People");
        }
    }
}
