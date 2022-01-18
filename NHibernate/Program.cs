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
                        using (var session = LoadTestContext.GetSessionFactory().OpenSession())
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
                                        Description = title,
                                    });
                                }
                            }

                            session.SaveOrUpdate(person);
                        }
                    }
                }
            }
            Console.WriteLine("Loading all rows from data.tsv file: finished {0}", DateTime.Now);
            goto readline;
            #endregion

            #region Updating all rows
            //second: updates all rows
            Console.WriteLine("Updating all rows from the People table: started {0}", DateTime.Now);
            {
                var people = LoadTestContext.GetSessionFactory().OpenSession().Query<Person>().ToList();
                foreach (var person in people)
                {
                    using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                    {
                        person.ColumnForUpdateTest = new string('0', 50);

                        foreach (var title in person.KnownForTitles)
                        {
                            title.ColumnForUpdateTest = new string('1', 50);
                            session.SaveOrUpdate(title);
                        }

                        foreach (var profession in person.PrimaryProfession)
                        {
                            profession.ColumnForUpdateTest = new string('2', 50);
                            session.SaveOrUpdate(profession);
                        }

                        session.SaveOrUpdate(person);
                    }
                }
            }
            Console.WriteLine("Updating all rows from the People table: finished {0}", DateTime.Now);
            #endregion

            #region Deleting all rows
            Console.WriteLine("Deleting all rows from the People table: started {0}", DateTime.Now);
            {
                var people = LoadTestContext.GetSessionFactory().OpenSession().Query<Person>().ToList();
                foreach (var person in people)
                {
                    using (var session = LoadTestContext.GetSessionFactory().OpenSession())
                    {
                        foreach (var title in person.KnownForTitles) session.Delete(title);
                        foreach (var profession in person.PrimaryProfession) session.Delete(profession);
                        session.Delete(person);
                    }
                }

            }
            Console.WriteLine("Deleting all rows from the People table: finished {0}", DateTime.Now);
            #endregion

            readline:
            Console.ReadLine();
        }
    }
}
