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
    using System.Data.SQLite;
    using System.IO;
    using System.Xml;

    using Inventar.DatabaseCore;
    using Inventar.Model;

    public class Program
    {
        private static string databasePath = string.Empty;

        private static void Main(string[] args)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            databasePath = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "_DemoData", "Inventar.db");


            ConsoleMenu.Add("01", "Erstellen Datenbank und Tabelle", () => MenuPoint01());
            ConsoleMenu.Add("02", "Tabellen InventarTyp füllen", () => MenuPoint02());
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
                Console.WriteLine($"Datenbank '{databasePath}' wurde noch nicht erstellt!!");
                ConsoleMenu.Wait();
                return;
            }

            using (DatabaseService ds = new DatabaseService(databasePath))
            {
                ds.Insert(InsertNewRow);
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
    }
}
