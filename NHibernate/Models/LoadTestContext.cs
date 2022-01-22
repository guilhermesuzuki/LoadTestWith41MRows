using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Models
{
    internal class LoadTestContext
    {
        private static ISessionFactory _sessionFactory;
        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory != null) return _sessionFactory;

            if (_sessionFactory == null)
            {
                _sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString(@"Server=localhost;Database=LoadTest_NHibernate;User Id=loadtest;Password=loadtest;").ShowSql())
                    .Mappings(c => c.FluentMappings
                        .Add<Mappings.PersonMap>()
                        .Add<Mappings.ProfessionMap>()
                        .Add<Mappings.TitleMap>())
                    .BuildSessionFactory();
            }

            return _sessionFactory;
        }
    }
}
