//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
// <Template>
// 	Version 2.0.2025.0, 28.4.2025
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.05.2025 19:34:00</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.ExpSoftware
{
    /* Imports from NET Framework */
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Xml;

    using ConsoleN;

    using Inventar.Database.Repository;
    using Inventar.DatabaseCore;
    using Inventar.Generator;
    using Inventar.Model;

    using Console = ConsoleN.Console;

    public class Program
    {
        private static string databasePath = string.Empty;
        private static string attachmentPath = string.Empty;
        private static string attachmentPath2 = string.Empty;

        private static void Main(string[] args)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            databasePath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "_DemoData", "Inventar.db");
            attachmentPath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "_DemoData", "AttachmentDemo.png");
            attachmentPath2 = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "_DemoData", "AttachmentDemo2.png");

            ConsoleMenu.Add("00", "ConsoleN", () => MenuPoint00());
            ConsoleMenu.Add("01", "Erstellen Datenbank und Tabelle", () => MenuPoint01());
            ConsoleMenu.Add("02", "Tabellen InventarTyp füllen", () => MenuPoint02());
            ConsoleMenu.Add("03", "Tabellen Attachment füllen", () => MenuPoint03());
            ConsoleMenu.Add("04", "Tabellen Insert/Update Inventar", () => MenuPoint04());
            ConsoleMenu.Add("05", "Tabellen Delete Inventar", () => MenuPoint05());
            ConsoleMenu.Add("06", "Repository Inventar", () => MenuPoint06());
            ConsoleMenu.Add("07", "Repository Attachment", () => MenuPoint07());
            ConsoleMenu.Add("08", "Repository Inventar Typen", () => MenuPoint08());
            ConsoleMenu.Add("A1", "DynamicSQL; Select", () => MenuPointA1());
            ConsoleMenu.Add("A2", "DynamicSQL; Join Tabellen", () => MenuPointA2());
            ConsoleMenu.Add("A3", "DynamicSQL; Insert Content", () => MenuPointA3());
            ConsoleMenu.Add("B1", "SqlBuilderContext", () => MenuPointB1());
            ConsoleMenu.Add("X", "Beenden", () => ApplicationExit());

            do
            {
                Console.Clear();
                Console.Titel("Menüauswahl");
                _ = ConsoleMenu.SelectKey(2, 2);
            }
            while (true);
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint00()
        {
            Console.Clear();

            /*
            Console.Info("Progress started...");
            Console.Warning("It seems there is issue in the system");
            Console.Error("Process failed :(");
            Console.Info("Retrying...");
            Console.ReadLine("Press enter to continue");
            Console.Success("Progress Succeed", showIcon: true);
            Console.WriteLine("Wait it is not completed yet", ConsoleColor.Magenta);
            */

            /*
            var value = Console.ReadLine("ProjectName:", "ConsoleR");
            if (!string.IsNullOrEmpty(value))
            {
                Console.AsciiArt(value, ConsoleColor.Green);
            }
            */

            /*
            string[] frontEndFrameworks = ["Blazor", "Angular", "Vue", "React", "VanillaJs"];
            var selectedItem = Console.Menu("Please Select One beloved frontend framework", true, frontEndFrameworks).Select();
            Console.AsciiArt(frontEndFrameworks[selectedItem], GetFrameworkColor(frontEndFrameworks[selectedItem]));
            */

            /*
            string[] frontEndFrameworks = ["Blazor", "Angular", "Vue", "React", "VanillaJs","AA","A1","A2","A3", "A4","A5","A6"];
            var selectedItem = Console.Menu("Please Select One beloved frontend framework", true, frontEndFrameworks).Select();
            */

            /*
            string[] plugins = ["TypeScript", "Linter", "Nuxt", "Vite"];
            var selectedItems = Console.Checkbox("Select feature that you want to install:", plugins).Select();
            for (int i = 0; i < selectedItems.Length; i++)
            {
                var plugin = selectedItems[i];
                Console.WriteLine(plugin.Option, (ConsoleColor)i);
            }
            */

            /*
            var password = Console.Password("Ihr Passwort:");
            Console.Alert($"Dein Passwort ist: {password}", "Passwort", ConsoleMessageType.Info);
            */

            /*
            Person[] people2 = [
                new Person("Gerhard",64, "Neuhofen"),
                new Person("Charlie", 2, "Constanza"),
                new Person("Buddy", 29, "Waldsee"),
                new Person("Beate", 64, "Neuhofen")
                ];

            Console.Table(people2, ConsoleColor.DarkCyan);
            */

            Console.Wait();
        }

        public record Person(string Name, int Age, string City);

        public static ConsoleColor GetFrameworkColor(string framework)
        {
            return framework switch
            {
                "Blazor" => ConsoleColor.DarkMagenta,
                "Angular" => ConsoleColor.Red,
                "Vue" => ConsoleColor.Green,
                "React" => ConsoleColor.Blue,
                "JS" => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
        }

        private static void MenuPoint01()
        {
            Console.Clear();

            if (File.Exists(databasePath) == true)
            {
                File.Delete(databasePath);
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Create(CreateTableInDB);
            }

            if (File.Exists(databasePath) == true)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde erstellt!!");
            }


            Console.Wait();
        }

        private static void MenuPoint02()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                Console.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Insert(InsertNewRow);
            }

            Console.Wait();
        }

        private static void MenuPoint03()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                Console.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Insert(InsertAttachment);
            }

            Console.Wait();
        }

        private static void MenuPoint04()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                Console.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Update(InsertUpdateInventarRow);
            }

            Console.Wait();
        }

        private static void MenuPoint05()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                Console.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Delete(DeleteInventarRow);
            }

            Console.Wait();
        }

        private static void MenuPoint06()
        {
            Console.Clear();

            using (InventarRepository<Inventars> repository = new InventarRepository<Inventars>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    Console.Wait();
                    return;
                }

                int countAll = repository.Count();
                if (countAll == 0)
                {
                    Inventars inv = new Inventars("Münzen 10 DM", new DateTime(1986, 1, 1), 560.00M);
                    inv.InventarTyp = 2;
                    repository.Add(inv);

                    inv = new Inventars("Münzen 5 DM", new DateTime(1960, 1, 1), 450.00M);
                    inv.InventarTyp = 2;
                    repository.Add(inv);

                    inv = new Inventars("Münzen 10 EUR", new DateTime(2001, 1, 1), 250.00M);
                    inv.InventarTyp = 2;
                    repository.Add(inv);

                    inv = new Inventars("Münzen verschiedene Länder", new DateTime(1975, 1, 1), 310.66M);
                    inv.InventarTyp = 2;
                    repository.Add(inv);

                    inv = new Inventars("Casio Wave Ceptor", new DateTime(2018, 6, 28), 298.85M);
                    inv.Ablageort = "in gebrauch";
                    inv.InventarTyp = 4;
                    repository.Add(inv);
                }
                else
                {
                    IEnumerable<Inventars> inventars = repository.Select();
                    IEnumerable<Inventars> inventarTyp = repository.SelectByInventarTyp(1);
                    repository.DeleteAll();
                }

                countAll = repository.Count();
                if (countAll > 0)
                {
                    Inventars inv0 = repository.Select().FirstOrDefault();
                    Console.WriteLine($"(Vor Update) Ablageort: {inv0.Ablageort}");

                    inv0.Ablageort = "Im Tresor";

                    repository.Update(inv0);

                    Inventars inv1 = repository.Select().FirstOrDefault();
                    Console.WriteLine($"(nach Update) Ablageort: {inv1.Ablageort}");

                    repository.Delete(inv1);
                    countAll = repository.Count();
                }
            }

            Console.Wait();
        }

        private static void MenuPoint07()
        {
            Console.Clear();

            using (AttachmentRepository<Attachments> repository = new AttachmentRepository<Attachments>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    Console.Wait();
                    return;
                }

                int countAll = repository.Count();
                if (countAll == 0)
                {
                    Attachments attachments = new Attachments();
                    attachments.Content = File.ReadAllBytes(attachmentPath);
                    attachments.FileExtension = Path.GetExtension(attachmentPath);
                    attachments.Filename = Path.GetFileName(attachmentPath);
                    attachments.FileSize = new FileInfo(attachmentPath).Length;
                    attachments.FileDateTime = new FileInfo(attachmentPath).LastWriteTime;
                    repository.Add(attachments);

                    attachments = new Attachments();
                    attachments.Content = File.ReadAllBytes(attachmentPath2);
                    attachments.FileExtension = Path.GetExtension(attachmentPath2);
                    attachments.Filename = Path.GetFileName(attachmentPath2);
                    attachments.FileSize = new FileInfo(attachmentPath2).Length;
                    attachments.FileDateTime = new FileInfo(attachmentPath2).LastWriteTime;
                    repository.Add(attachments);
                }
                else
                {
                    //repository.DeleteAll();
                    IEnumerable<Attachments> attachments = repository.Select();
                }
            }

            Console.Wait();
        }

        private static void MenuPoint08()
        {
            Console.Clear();

            using (InventarTypRepository<InventarTyp> repository = new InventarTypRepository<InventarTyp>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    Console.Wait();
                    return;
                }

                int countAll = repository.Count();
                if (countAll == 0)
                {
                    InventarTyp invTyp = new InventarTyp("Keine Auswahl", 1);
                    repository.Add(invTyp);

                    invTyp = new InventarTyp("Münzen, Medaillen, Brifmarken", 2);
                    repository.Add(invTyp);

                    invTyp = new InventarTyp("Schmuck, Minerale, Edelsteine", 3);
                    repository.Add(invTyp);

                    invTyp = new InventarTyp("Uhren", 4);
                    repository.Add(invTyp);

                    invTyp = new InventarTyp("Dokumente", 5);
                    repository.Add(invTyp);

                    invTyp = new InventarTyp("Bilder", 6);
                    repository.Add(invTyp);

                    DataRow dr = repository.NewDataRow();
                    dr.SetField<Guid>("Id", Guid.NewGuid());
                    dr.SetField<string>("Name", "Verschiedenes");
                    dr.SetField<string>("Description", "Alles was nicht rein passt");
                    dr.SetField<int>("Typ", 7);
                    dr.SetField<bool>("IsActive", false);
                    dr.SetField<string>("CreatedBy", UserInfo.TS().CurrentUser);
                    dr.SetField<DateTime>("CreatedOn", UserInfo.TS().CurrentTime);
                    repository.Add(dr);

                }
                else
                {
                    //repository.DeleteAll();
                    IEnumerable<InventarTyp> inventarTypen = repository.Select();
                }
            }

            Console.Wait();
        }

        private static void MenuPointA1()
        {
            Console.Clear();
            string sqlStatment = string.Empty;

            Console.Titel("Select alle Spalten");
            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                query.SelectFromTable("TAB_Inventar");
                query.SelectAllColumns();
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Titel("Select alle Spalten und Order By");
            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                query.SelectFromTable("TAB_Inventar");
                query.SelectAllColumns();
                query.AddOrderBy("Name", SqlSorting.Ascending);
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Titel("Select alle Spalten und Where-Bedingung");
            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                query.SelectFromTable("TAB_Inventar");
                query.SelectAllColumns();
                query.AddWhere("InventarTyp", SqlComparison.Equals, 1);
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Titel("Select alle Spalten und Where-Or-Bedingung");
            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                WhereClause wcId = new WhereClause("Typ", SqlComparison.Equals, 1);
                wcId.AddClause(SqlLogicOperator.Or, SqlComparison.Equals, 2);

                query.SelectFromTable("TAB_Inventar");
                query.SelectColumns("Id", "Name");
                query.AddWhere(wcId);
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Wait();
        }

        private static void MenuPointA2()
        {
            Console.Clear();
            string sqlStatment = string.Empty;

            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                query.SelectFromTable("TAB_Inventar");
                query.SelectColumns($"{"TAB_Inventar"}.ID", $"{"TAB_Inventar"}.Name", $"{"TAB_Inventar"}.KaufBetrag", $"{"TAB_Inventar"}.GekauftAm", "TAB_InventarTyp.Name");
                query.AddJoin(SqlJoinType.LeftJoin, "TAB_InventarTyp", "Typ", SqlComparison.Equals, "TAB_Inventar", "InventarTyp");
                query.AddOrderBy($"{"TAB_Inventar"}.Name", SqlSorting.Ascending);
                query.AddOrderBy("TAB_InventarTyp.Typ", SqlSorting.Ascending);
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Wait();
        }

        private static void MenuPointA3()
        {
            Console.Clear();
            string sqlStatment = string.Empty;

            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                query.InsertIntoTable("TAB_InventarTyp");
                query.AddColumn("Id", "@Id");
                query.AddColumn("Name", "@Name");
                query.AddColumn("Description", "@Description");
                query.AddColumn("Typ", "@Typ");
                query.AddColumn("IsActive", "@IsActive");
                query.AddColumn("CreatedBy", "@CreatedBy");
                query.AddColumn("CreatedOn", "@CreatedOn");
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Wait();
        }

        private static void MenuPointA4()
        {
            Console.Clear();
            string sqlStatment = string.Empty;

            using (DynamicSQLBuilder query = new DynamicSQLBuilder())
            {
                query.InsertIntoTable("TAB_InventarTyp");
                query.AddColumn("Id", "@Id");
                query.AddColumn("Name", "@Name");
                query.AddColumn("Description", "@Description");
                query.AddColumn("Typ", "@Typ");
                query.AddColumn("IsActive", "@IsActive");
                query.AddColumn("CreatedBy", "@CreatedBy");
                query.AddColumn("CreatedOn", "@CreatedOn");
                sqlStatment = query.BuildQuery();
            }

            Console.Continue(sqlStatment);

            Console.Wait();
        }

        private static void MenuPointB1()
        {
            Console.Clear();

            using (InventarTypRepository<InventarTyp> repository = new InventarTypRepository<InventarTyp>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    Console.Wait();
                    return;
                }

                //repository.AddDataRow();
                //repository.UpdateDataRow();
                //_ = repository.DeleteDataRow();
            }

            Console.Wait();
        }

        private static void CreateTableInDB(SQLiteConnection sqliteConnection)
        {
            SQLGenerator<Inventars> createInventar = new SQLGenerator<Inventars>(null);
            string resultSql = createInventar.CreateTable().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            SQLGenerator<InventarTyp> createInventarTyp = new SQLGenerator<InventarTyp>(null);
            resultSql = createInventarTyp.CreateTable().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            SQLGenerator<Attachments> createAttachments = new SQLGenerator<Attachments>(null);
            resultSql = createAttachments.CreateTable().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);
        }

        private static void InsertNewRow(SQLiteConnection sqliteConnection)
        {
            InventarTyp invTyp = new InventarTyp("Keine Auswahl", 1);
            SQLGenerator<InventarTyp> insertInvTyp = new SQLGenerator<InventarTyp>(invTyp);
            string resultSql = insertInvTyp.Insert().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            invTyp = new InventarTyp("Münzen, Medaillen, Brifmarken", 2);
            insertInvTyp = new SQLGenerator<InventarTyp>(invTyp);
            resultSql = insertInvTyp.Insert().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            invTyp = new InventarTyp("Schmuck, Minerale, Edelsteine", 3);
            insertInvTyp = new SQLGenerator<InventarTyp>(invTyp);
            resultSql = insertInvTyp.Insert().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            invTyp = new InventarTyp("Uhren", 4);
            insertInvTyp = new SQLGenerator<InventarTyp>(invTyp);
            resultSql = insertInvTyp.Insert().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            invTyp = new InventarTyp("Dokumente", 5);
            insertInvTyp = new SQLGenerator<InventarTyp>(invTyp);
            resultSql = insertInvTyp.Insert().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);

            invTyp = new InventarTyp("Bilder", 6);
            insertInvTyp = new SQLGenerator<InventarTyp>(invTyp);
            resultSql = insertInvTyp.Insert().ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);
        }

        private static void InsertAttachment(SQLiteConnection sqliteConnection)
        {
            Attachments attachments = new Attachments();
            attachments.Content = null;
            attachments.FileExtension = Path.GetExtension(attachmentPath);
            attachments.Filename = Path.GetFileName(attachmentPath);
            attachments.FileSize = new FileInfo(attachmentPath).Length;
            attachments.FileDateTime = new FileInfo(attachmentPath).LastWriteTime;
            SQLGenerator<Attachments> insertInvTyp = new SQLGenerator<Attachments>(attachments);
            string resultSql = insertInvTyp.Insert().ToSql();
            Dictionary<string, object> parameterCollection = new Dictionary<string, object>();
            parameterCollection.Add("@Content", File.ReadAllBytes(attachmentPath));
            sqliteConnection.CmdExecuteNonQuery(resultSql, parameterCollection);
        }

        private static void InsertUpdateInventarRow(SQLiteConnection sqliteConnection)
        {
            SQLGenerator<Inventars> selectInv = new SQLGenerator<Inventars>(null);
            string resultSql = selectInv.Select(SQLSelectOperator.Count).ToSql();
            int count = sqliteConnection.CmdExecuteScalar<int>(resultSql);
            if (count == 0)
            {
                Inventars inv = new Inventars("Münzen 10 DM", new DateTime(1986, 1, 1), 150.66M);
                SQLGenerator<Inventars> insertInv = new SQLGenerator<Inventars>(inv);
                resultSql = insertInv.Insert(true).ToSql();
                sqliteConnection.CmdExecuteNonQuery(resultSql);
            }

            resultSql = selectInv.Select().Take(1).ToSql();
            DataTable tableInv = sqliteConnection.CmdReaderToDataTable(resultSql);
            IEnumerable<Inventars> entityInv = sqliteConnection.CmdReaderToMap<Inventars>(resultSql);

            Inventars invUpdate = entityInv.FirstOrDefault();
            invUpdate.GekauftAm = new DateTime(1984, 6, 28);
            invUpdate.ModifiedBy = UserInfo.TS().CurrentUser;
            invUpdate.ModifiedOn = UserInfo.TS().CurrentTime;
            SQLGenerator<Inventars> updateInv = new SQLGenerator<Inventars>(invUpdate);
            resultSql = updateInv.Update(u => u.GekauftAm, u => u.InventarAlter, u => u.ModifiedBy, u => u.ModifiedOn).Where(w => w.Id,SqlComparison.Equals, invUpdate.Id).ToSql();
            sqliteConnection.CmdExecuteNonQuery(resultSql);
        }

        private static void DeleteInventarRow(SQLiteConnection sqliteConnection)
        {
            SQLGenerator<Inventars> selectInv = new SQLGenerator<Inventars>(null);
            string resultSql = selectInv.Select().Take(1).ToSql();
            DataTable tableInv = sqliteConnection.CmdReaderToDataTable(resultSql);
            IEnumerable<Inventars> entityInv = sqliteConnection.CmdReaderToMap<Inventars>(resultSql);
            Inventars invDelete = entityInv.FirstOrDefault();

            SQLGenerator<Inventars> deleteInv = new SQLGenerator<Inventars>(invDelete);
            resultSql = deleteInv.Delete(d => d.Id).ToSql();
            int deleteCount = sqliteConnection.CmdExecuteNonQuery(resultSql);
        }
    }
}
