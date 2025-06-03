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

    using Inventar.Database.Repository;
    using Inventar.DatabaseCore;
    using Inventar.Generator;
    using Inventar.Model;

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

            ConsoleMenu.Add("01", "Erstellen Datenbank und Tabelle", () => MenuPoint01());
            ConsoleMenu.Add("02", "Tabellen InventarTyp füllen", () => MenuPoint02());
            ConsoleMenu.Add("03", "Tabellen Attachment füllen", () => MenuPoint03());
            ConsoleMenu.Add("04", "Tabellen Insert/Update Inventar", () => MenuPoint04());
            ConsoleMenu.Add("05", "Tabellen Delete Inventar", () => MenuPoint05());
            ConsoleMenu.Add("06", "Repository Inventar", () => MenuPoint06());
            ConsoleMenu.Add("07", "Repository Attachment", () => MenuPoint07());
            ConsoleMenu.Add("08", "Repository Inventar Typen", () => MenuPoint08());
            ConsoleMenu.Add("X", "Beenden", () => ApplicationExit());

            do
            {
                _ = ConsoleMenu.SelectKey(2, 2);
            }
            while (true);
        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
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


            ConsoleMenu.Wait();
        }

        private static void MenuPoint02()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                ConsoleMenu.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Insert(InsertNewRow);
            }

            ConsoleMenu.Wait();
        }

        private static void MenuPoint03()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                ConsoleMenu.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Insert(InsertAttachment);
            }

            ConsoleMenu.Wait();
        }

        private static void MenuPoint04()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                ConsoleMenu.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Update(InsertUpdateInventarRow);
            }

            ConsoleMenu.Wait();
        }

        private static void MenuPoint05()
        {
            Console.Clear();

            if (File.Exists(databasePath) == false)
            {
                Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                ConsoleMenu.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Delete(DeleteInventarRow);
            }

            ConsoleMenu.Wait();
        }

        private static void MenuPoint06()
        {
            Console.Clear();

            using (InventarRepository<Inventars> repository = new InventarRepository<Inventars>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    ConsoleMenu.Wait();
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

            ConsoleMenu.Wait();
        }

        private static void MenuPoint07()
        {
            Console.Clear();

            using (AttachmentRepository<Attachments> repository = new AttachmentRepository<Attachments>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    ConsoleMenu.Wait();
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

            ConsoleMenu.Wait();
        }

        private static void MenuPoint08()
        {
            Console.Clear();

            using (InventarTypRepository<InventarTyp> repository = new InventarTypRepository<InventarTyp>())
            {
                if (repository.Exist() == false)
                {
                    Console.WriteLine($"Datenbank '{databasePath}' wurde nicht gefunden oder erstellt!");
                    ConsoleMenu.Wait();
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
                }
                else
                {
                    //repository.DeleteAll();
                    IEnumerable<InventarTyp> inventarTypen = repository.Select();
                }
            }

            ConsoleMenu.Wait();
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
            resultSql = updateInv.Update(u => u.GekauftAm, u => u.InventarAlter, u => u.ModifiedBy, u => u.ModifiedOn).Where(w => w.Id,SQLComparison.Equals, invUpdate.Id).ToSql();
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
