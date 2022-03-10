// See https://aka.ms/new-console-template for more information
using EFCore7.Models;
using Microsoft.EntityFrameworkCore;

#region Loading all rows
//first: load all rows from the text file into the database table
var filepath = Path.GetFullPath("Files\\data.tsv");
Console.WriteLine("Loading all rows from data.tsv file: started {0}", DateTime.Now);
using (var stream = new StreamReader(filepath))
{
    stream.ReadLine();
    while (stream.EndOfStream == false)
    {
        using (var db = new LoadTestContext())
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

                db.People.Add(person);
                db.SaveChanges();
            }
        }
    }
}
Console.WriteLine("Loading all rows from data.tsv file: finished {0}", DateTime.Now);
#endregion

#region Updating all rows
update:
//second: updates all rows
Console.WriteLine("Updating all rows from the People, Professions and Titles tables: started {0}", DateTime.Now);
{
    var count = new LoadTestContext().People.Count();
    var index = 0;

    while (index < count)
    {
        var people = new LoadTestContext().People
            .Include(x => x.PrimaryProfession)
            .Include(x => x.KnownForTitles)
            .Skip(index)
            .Take(1000000)
            .ToList();

        foreach (var person in people)
        {
            using (var db = new LoadTestContext())
            {
                person.ColumnForUpdateTest = new string('0', 50);
                db.Attach(person);
                db.Entry(person).Property(x => x.ColumnForUpdateTest).IsModified = true;
                db.SaveChanges();

                foreach (var title in person.KnownForTitles)
                {
                    title.ColumnForUpdateTest = new string('1', 50);
                    db.Attach(title);
                    db.Entry(title).Property(x => x.ColumnForUpdateTest).IsModified = true;
                    db.SaveChanges();
                }

                foreach (var profession in person.PrimaryProfession)
                {
                    profession.ColumnForUpdateTest = new string('2', 50);
                    db.Attach(profession);
                    db.Entry(profession).Property(x => x.ColumnForUpdateTest).IsModified = true;
                    db.SaveChanges();
                }
            }
        }

        index += 1000000;
    }
}
Console.WriteLine("Updating all rows from the People, Professions and Titles tables: finished {0}", DateTime.Now);
#endregion

#region Deleting all rows
delete:
Console.WriteLine("Deleting all rows from the People, Professions and Titles tables: started {0}", DateTime.Now);
{
    var count = new LoadTestContext().People.Count();
    var index = 0;

    while (index < count)
    {
        var people = new LoadTestContext().People
            .Include(x => x.PrimaryProfession)
            .Include(x => x.KnownForTitles)
            .Take(1000000)
            .ToList();

        foreach (var person in people)
        {
            using (var db = new LoadTestContext())
            {
                db.RemoveRange(person.KnownForTitles);
                db.RemoveRange(person.PrimaryProfession);
                db.Remove(person);
                db.SaveChanges();
            }
        }

        index += 1000000;
    }
}
Console.WriteLine("Deleting all rows from the People, Professions and Titles tables: finished {0}", DateTime.Now);
#endregion

readline:
Console.ReadLine();