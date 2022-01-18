#region Loading all rows
//first: load all rows from the text file into the database table
using AdoNet.Models;
using System.Data.SqlClient;

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

                LoadTestContext.InsertPerson(conn, person);
            }
        }

        conn.Close();
    }
}
Console.WriteLine("Loading all rows from data.tsv file: finished {0}", DateTime.Now);
#endregion

#region Updating all rows
//second: updates all rows
Console.WriteLine("Updating all rows from the People, Professions and Titles tables: started {0}", DateTime.Now);
{
    using (var conn = new SqlConnection(LoadTestContext.ConnectionString))
    {
        conn.Open();

        var people = LoadTestContext.SelectPeople(conn);
        foreach (var person in people)
        {
            person.ColumnForUpdateTest = new string('0', 50);

            foreach (var title in person.KnownForTitles)
            {
                title.ColumnForUpdateTest = new string('1', 50);
            }

            foreach (var profession in person.PrimaryProfession)
            {
                profession.ColumnForUpdateTest = new string('2', 50);
            }

            LoadTestContext.UpdatePerson(conn, person);
        }

        conn.Close();
    }
}
Console.WriteLine("Updating all rows from the People, Professions and Titles tables: finished {0}", DateTime.Now);
#endregion

#region Deleting all rows
Console.WriteLine("Deleting all rows from the People, Professions and Titles tables: started {0}", DateTime.Now);
{
    using (var conn = new SqlConnection(LoadTestContext.ConnectionString))
    {
        conn.Open();

        var people = LoadTestContext.SelectPeople(conn);
        foreach (var person in people)
        {
            LoadTestContext.DeletePerson(conn, person);
        }

        conn.Close();
    }
}
Console.WriteLine("Deleting all rows from the People, Professions and Titles tables: finished {0}", DateTime.Now);
#endregion

Console.ReadLine();
