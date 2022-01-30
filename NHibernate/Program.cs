using NHibernate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Loading all rows
            //first: load all rows from the text file into the database table
            var filepath = Path.GetFullPath("Files\\data.tsv");
            Console.WriteLine("Loading all rows from data.tsv file: started {0}", DateTime.Now);
            using (var stream = new StreamReader(filepath))
            {
                stream.ReadLine();
                while (stream.EndOfStream == false)
                {
                    var line = stream.ReadLine();
                    if (string.IsNullOrWhiteSpace(line) == false)
                    {
                        var fields = line.Split('\t');
                        var person = new Person()
                        {
                            NConst = fields[0],
                            PrimaryName = fields[1],
                            BirthYear = fields[2] == String.Empty || fields[2] == @"\N" ? (short?)null : short.Parse(fields[2]),
                            DeathYear = fields[3] == String.Empty || fields[3] == @"\N" ? (short?)null : short.Parse(fields[3]),
                        };

                        if (string.IsNullOrWhiteSpace(fields[4]) == false && fields[4] != @"\N")
                        {
                            var professions = fields[4].Split(',');
                            foreach (var profession in professions)
                            {
                                person.PrimaryProfession.Add(new Profession()
                                {
                                    NConst = person.NConst,
                                    Description = profession,
                                });
                            }
                        }

                        if (string.IsNullOrWhiteSpace(fields[5]) == false && fields[5] != @"\N")
                        {
                            var titles = fields[5].Split(',');
                            foreach (var title in titles)
                            {
                                person.KnownForTitles.Add(new Title()
                                {
                                    NConst = person.NConst,
                                    Description = title,
                                });
                            }
                        }

                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            session.Save(person);
                            session.Flush();
                            session.Close();
                        }

                        foreach (var profession in person.PrimaryProfession)
                        {
                            using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                            {
                                session.Save(profession);
                                session.Flush();
                                session.Close();
                            }
                        }

                        foreach (var title in person.KnownForTitles)
                        {
                            using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                            {
                                session.Save(title);
                                session.Flush();
                                session.Close();
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Loading all rows from data.tsv file: finished {0}", DateTime.Now);
        #endregion

        #region Updating all rows
        update:
            //second: updates all rows
            Console.WriteLine("Updating all rows from the People table: started {0}", DateTime.Now);
            {
                var count = LoadTestContext.GetSessionFactory().OpenStatelessSession().Query<Person>().Count();
                var index = 0;
                while (index < count)
                {
                    var people = LoadTestContext.GetSessionFactory()
                        .OpenStatelessSession()
                        .Query<Person>()
                        .Skip(index)
                        .Take(1000000)
                        .ToList();

                    foreach (var person in people)
                    {
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            person.ColumnForUpdateTest = new string('0', 50);
                            session.Update(person);
                            session.Flush();
                            session.Close();
                        }
                    }

                    index += 1000000;
                }

                count = LoadTestContext.GetSessionFactory().OpenStatelessSession().Query<Profession>().Count();
                index = 0;
                while (index < count)
                {
                    var professions = LoadTestContext.GetSessionFactory()
                        .OpenStatelessSession()
                        .Query<Profession>()
                        .Skip(index)
                        .Take(1000000)
                        .ToList();

                    foreach (var profession in professions)
                    {
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            profession.ColumnForUpdateTest = new string('1', 50);
                            session.Update(profession);
                            session.Flush();
                            session.Close();
                        }

                    }

                    index += 1000000;
                }

                count = LoadTestContext.GetSessionFactory().OpenStatelessSession().Query<Title>().Count();
                index = 0;
                while (index < count)
                {
                    var titles = LoadTestContext.GetSessionFactory()
                        .OpenStatelessSession()
                        .Query<Title>()
                        .Skip(index)
                        .Take(1000000)
                        .ToList();

                    foreach (var title in titles)
                    {
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            title.ColumnForUpdateTest = new string('2', 50);
                            session.SaveOrUpdate(title);
                            session.Flush();
                            session.Close();
                        }
                    }

                    index += 1000000;
                }
            }

            Console.WriteLine("Updating all rows from the People table: finished {0}", DateTime.Now);
            #endregion

        #region Deleting all rows
        delete:
            Console.WriteLine("Deleting all rows from the People table: started {0}", DateTime.Now);
            {
                var count = LoadTestContext.GetSessionFactory().OpenStatelessSession().Query<Profession>().Count();
                var index = 0;
                while (index < count)
                {
                    var professions = LoadTestContext.GetSessionFactory()
                        .OpenStatelessSession()
                        .Query<Profession>()
                        .Skip(0)
                        .Take(1000000)
                        .ToList();

                    foreach (var profession in professions)
                    {
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            session.Delete(profession);
                            session.Flush();
                            session.Close();
                        }
                    }

                    index += 1000000;
                }

                count = LoadTestContext.GetSessionFactory().OpenStatelessSession().Query<Title>().Count();
                index = 0;
                while (index < count)
                {
                    var titles = LoadTestContext.GetSessionFactory()
                        .OpenStatelessSession()
                        .Query<Title>()
                        .Skip(0)
                        .Take(1000000)
                        .ToList();

                    foreach (var title in titles)
                    {
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            session.Delete(title);
                            session.Flush();
                            session.Close();
                        }
                    }

                    index += 1000000;
                }

                count = LoadTestContext.GetSessionFactory().OpenStatelessSession().Query<Person>().Count();
                index = 0;
                while (index < count)
                {
                    var people = LoadTestContext.GetSessionFactory()
                        .OpenStatelessSession()
                        .Query<Person>()
                        .Skip(0)
                        .Take(500000)
                        .ToList();

                    foreach (var person in people)
                    {
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                        {
                            session.Delete(person);
                            session.Flush();
                            session.Close();
                        }
                    }

                    index += 500000;
                }
            }
            Console.WriteLine("Deleting all rows from the People table: finished {0}", DateTime.Now);
        #endregion

        readline:
            Console.ReadLine();
        }
    }
}
