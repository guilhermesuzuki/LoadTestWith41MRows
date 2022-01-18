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
            Id(x => x.NConst);
            Map(x => x.PrimaryName).Length(250);
            Map(x => x.BirthYear).Nullable();
            Map(x => x.DeathYear).Nullable();
            Map(x => x.ColumnForUpdateTest).Nullable();
            HasMany(x => x.PrimaryProfession).Not.LazyLoad().KeyColumn("NConst");
            HasMany(x => x.KnownForTitles).Not.LazyLoad().KeyColumn("NConst");
            Table("People");
        }
    }
}
