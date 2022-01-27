using AdoNet.Models;
using System.Data.SqlClient;

goto delete;

#region Loading all rows
//first: load all rows from the text file into the database table
var filepath = Path.GetFullPath("Files\\data.tsv");
Console.WriteLine("Loading all rows from data.tsv file: started {0}", DateTime.Now);
using (var stream = new StreamReader(filepath))
{
    stream.ReadLine();
    using (var conn = new SqlConnection(LoadTestContext.ConnectionString))
    {
        conn.Open();

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
                    BirthYear = fields[2] == String.Empty || fields[2] == @"\N" ? null : short.Parse(fields[2]),
                    DeathYear = fields[3] == String.Empty || fields[3] == @"\N" ? null : short.Parse(fields[3]),
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

                LoadTestContext.InsertPerson(conn, person);
            }
        }

        conn.Close();
    }
}
Console.WriteLine("Loading all rows from data.tsv file: finished {0}", DateTime.Now);
goto readline;
#endregion

#region Updating all rows
update:
//second: updates all rows
Console.WriteLine("Updating all rows from the People, Professions and Titles tables: started {0}", DateTime.Now);
{
    using (var conn = new SqlConnection(LoadTestContext.ConnectionString))
    {
        conn.Open();

        var count = LoadTestContext.CountPeople(conn);
        var index = 0;
        while (index < count)
        {
            var people = LoadTestContext.SelectPeople(conn, index, 1000000);
            foreach (var person in people)
            {
                person.ColumnForUpdateTest = new string('0', 50);
                LoadTestContext.UpdatePerson(conn, person);
            }

            index += 1000000;
        }

        count = LoadTestContext.CountProfessions(conn);
        index = 0;
        while (index < count)
        {
            var professions = LoadTestContext.SelectProfessions(conn, index, 1000000);
            foreach (var profession in professions)
            {
                profession.ColumnForUpdateTest = new string('0', 50);
                LoadTestContext.UpdateProfession(conn, profession);
            }

            index += 1000000;
        }

        count = LoadTestContext.CountTitles(conn);
        index = 0;
        while (index < count)
        {
            var titles = LoadTestContext.SelectTitles(conn, index, 1000000);
            foreach (var title in titles)
            {
                title.ColumnForUpdateTest = new string('0', 50);
                LoadTestContext.UpdateTitle(conn, title);
            }

            index += 1000000;
        }

        conn.Close();
    }
}
Console.WriteLine("Updating all rows from the People, Professions and Titles tables: finished {0}", DateTime.Now);
#endregion
goto readline;

#region Deleting all rows
delete:
Console.WriteLine("Deleting all rows from the People, Professions and Titles tables: started {0}", DateTime.Now);
{
    using (var conn = new SqlConnection(LoadTestContext.ConnectionString))
    {
        conn.Open();

        var count = LoadTestContext.CountProfessions(conn);
        var index = 0;
        while (index < count)
        {
            var professions = LoadTestContext.SelectProfessions(conn, 0, 1000000);
            foreach (var profession in professions)
            {
                LoadTestContext.DeleteProfession(conn, profession);
            }

            index += 1000000;
        }

        count = LoadTestContext.CountTitles(conn);
        index = 0;
        while (index < count)
        {
            var titles = LoadTestContext.SelectTitles(conn, 0, 1000000);
            foreach (var title in titles)
            {
                LoadTestContext.DeleteTitle(conn, title);
            }

            index += 1000000;
        }

        count = LoadTestContext.CountPeople(conn);
        index = 0;
        while (index < count)
        {
            var people = LoadTestContext.SelectPeople(conn, 0, 1000000, false);
            foreach (var person in people)
            {
                LoadTestContext.DeletePerson(conn, person);
            }

            index += 1000000;
        }

        conn.Close();
    }
}
Console.WriteLine("Deleting all rows from the People, Professions and Titles tables: finished {0}", DateTime.Now);
#endregion

readline:
Console.ReadLine();
