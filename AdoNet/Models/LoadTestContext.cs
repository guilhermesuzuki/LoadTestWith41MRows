using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNet.Models
{
    internal class LoadTestContext
    {
        public static string ConnectionString = "Server=localhost;Database=LoadTest_AdoNet;User Id=loadtest;Password=loadtest;";

        #region Insert methods

        public static void InsertPerson(SqlConnection conn, Person p)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [dbo].People (NConst,PrimaryName,BirthYear,DeathYear) VALUES (@NConst,@PrimaryName,@BirthYear,@DeathYear);";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = p.NConst });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@PrimaryName", Value = p.PrimaryName });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@BirthYear", Value = p.BirthYear });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@DeathYear", Value = p.DeathYear });
                    cmd.ExecuteNonQuery();
                }

                foreach (var profession in p.PrimaryProfession) InsertProfession(conn, profession);
                foreach (var title in p.KnownForTitles) InsertTitle(conn, title);
            }
        }

        public static int InsertProfession(SqlConnection conn, Profession p)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [dbo].Professions (NConst,Description) VALUES (@NConst,@Description); SELECT IDENT_CURRENT('Professions');";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = p.NConst });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Description", Value = p.Description });
                    p.Id = (int)cmd.ExecuteScalar();
                    return p.Id;
                }
            }

            return 0;
        }

        public static int InsertTitle(SqlConnection conn, Title t)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO [dbo].Titles (NConst,Description) VALUES (@NConst,@Description); SELECT IDENT_CURRENT('Titles');";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = t.NConst });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Description", Value = t.Description });
                    t.Id = (int)cmd.ExecuteScalar();
                    return t.Id;
                }
            }

            return 0;
        }

        #endregion

        #region Update methods

        public static void UpdatePerson(SqlConnection conn, Person p)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE [dbo].People SET ColumnForUpdateTest = @ColumnForUpdateTest WHERE NConst = @NConst;";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = p.NConst });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ColumnForUpdateTest", Value = p.ColumnForUpdateTest });
                    cmd.ExecuteNonQuery();
                }

                foreach (var profession in p.PrimaryProfession) UpdateProfession(conn, profession);
                foreach (var title in p.KnownForTitles) UpdateTitle(conn, title);
            }
        }

        public static void UpdateProfession(SqlConnection conn, Profession p)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE [dbo].Professions SET ColumnForUpdateTest = @ColumnForUpdateTest WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Id", Value = p.Id });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ColumnForUpdateTest", Value = p.ColumnForUpdateTest });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateTitle(SqlConnection conn, Title t)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE [dbo].Titles SET ColumnForUpdateTest = @ColumnForUpdateTest WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Id", Value = t.Id });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ColumnForUpdateTest", Value = t.ColumnForUpdateTest });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Delete methods

        public static void DeletePerson(SqlConnection conn, Person p)
        {
            foreach (var profession in p.PrimaryProfession) DeleteProfession(conn, profession);
            foreach (var title in p.KnownForTitles) DeleteTitle(conn, title);

            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [dbo].People WHERE NConst = @NConst;";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = p.NConst });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteProfession(SqlConnection conn, Profession p)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [dbo].Professions WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Id", Value = p.Id });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteTitle(SqlConnection conn, Title t)
        {
            if (conn != null)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM [dbo].Titles WHERE Id = @Id;";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Id", Value = t.Id });
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Select methods

        public static List<Person> SelectPeople(SqlConnection conn)
        {
            if (conn != null)
            {
                var people = new List<Person>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [dbo].People";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var person = new Person()
                            {
                                NConst = reader["NConst"] != null ? reader["NConst"].ToString() : String.Empty,
                                PrimaryName = reader["PrimaryName"] != null ? reader["PrimaryName"].ToString() : String.Empty,
                                BirthYear = reader["BirthYear"] != null ? (short?)int.Parse(reader["BirthYear"].ToString()) : 0,
                                DeathYear = reader["DeathYear"] != null ? (short?)int.Parse(reader["DeathYear"].ToString()) : 0,
                            };

                            person.PrimaryProfession = SelectProfessions(conn, person.NConst);
                            person.KnownForTitles = SelectTitles(conn, person.NConst);

                            people.Add(person);
                        }
                    }
                }
            }

            return new List<Person>();
        }

        public static List<Profession> SelectProfessions(SqlConnection conn, string NConst)
        {
            if (conn != null)
            {
                var professions = new List<Profession>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [dbo].Professions WHERE NConst = @NConst";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = NConst });
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var profession = new Profession()
                            {
                                Id = reader["Id"] != null ? int.Parse(reader["Id"].ToString()) : 0,
                                NConst = reader["NConst"] != null ? reader["NConst"].ToString() : String.Empty,
                                Description = reader["Description"] != null ? reader["Description"].ToString() : String.Empty,
                            };
                            professions.Add(profession);
                        }
                    }
                }
                return professions;
            }

            return new List<Profession>();
        }

        public static List<Title> SelectTitles(SqlConnection conn, string NConst)
        {
            if (conn != null)
            {
                var titles = new List<Title>();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM [dbo].Professions WHERE NConst = @NConst";
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NConst", Value = NConst });
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var title = new Title()
                            {
                                Id = reader["Id"] != null ? int.Parse(reader["Id"].ToString()) : 0,
                                NConst = reader["NConst"] != null ? reader["NConst"].ToString() : String.Empty,
                                Description = reader["Description"] != null ? reader["Description"].ToString() : String.Empty,
                            };
                            titles.Add(title);
                        }
                    }
                }
                return titles;
            }

            return new List<Title>();
        } 

        #endregion
    }
}
