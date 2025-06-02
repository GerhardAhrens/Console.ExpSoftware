//-----------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Lifeprojects.de">
//     Class: RepositoryBase
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.06.2025 08:00:03</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.DatabaseCore
{
    using System;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;

    using Inventar.Generator;

    public abstract class RepositoryBase<TEntity> : IDisposable
    {
        private bool classIsDisposed = false;

        protected RepositoryBase(string connectionInfo = "") : this()
        {
            this.Init(connectionInfo);
        }

        protected RepositoryBase()
        {
            this.Init(null);
        }

        public string FullName { get; private set; }

        public string Database { get; private set; }

        public string SqlConnectionString { get; private set; }

        public bool IsOpen { get; private set; }

        public SQLiteConnection Connection { get; private set; }

        public bool Exist()
        {
            if (File.Exists(this.FullName) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public SQLiteConnection OpenConnection()
        {
            try
            {
                if (File.Exists(this.FullName) == true)
                {
                    SQLiteConnection sqliteConnection = new SQLiteConnection(this.SqlConnectionString);
                    if (sqliteConnection.State != ConnectionState.Open)
                    {
                        sqliteConnection.Open();
                        this.Connection = sqliteConnection;
                        this.IsOpen = true;
                    }
                }
            }
            catch (Exception ex) when (ex is SQLiteException)
            {
                string errorText = ex.Message;
                throw new SQLiteException($"Es ist ein Problem Datenbankdatei '{this.FullName}' aufgetreten!", ex);
            }

            return this.Connection;
        }

        public void CloseConnection()
        {
            try
            {
                if (File.Exists(this.FullName) == true)
                {
                    if (this.Connection.State == ConnectionState.Open)
                    {
                        this.Connection.Close();
                        this.FullName = null;
                        this.Database = null;
                        this.SqlConnectionString = null;
                        this.IsOpen = false;
                        this.Connection = null;
                    }
                }
            }
            catch (Exception ex) when (ex is SQLiteException)
            {
                string errorText = ex.Message;
                throw new SQLiteException($"Es ist ein Problem Datenbankdatei '{this.FullName}' aufgetreten!", ex);
            }
        }

        public string DataTableName()
        {
            string tableName = string.Empty;
            tableName = typeof(TEntity).GetAttributeValue((DataTableAttribute table) => table.TableName);
            if (string.IsNullOrEmpty(tableName) == true)
            {
                tableName = typeof(TEntity).Name;
            }

            return tableName;
        }

        private void Init(string connectionInfo)
        {
            try
            {
                this.IsOpen = false;
                if (string.IsNullOrEmpty(connectionInfo) == false)
                {
                    this.FullName = connectionInfo;
                    this.Database = Path.GetFileName(connectionInfo);
                    this.SqlConnectionString = this.ConnectStringToText(this.FullName);
                }
                else
                {
                    string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    this.FullName = Path.Combine(new DirectoryInfo(currentDirectory).Parent.Parent.Parent.FullName, "_DemoData", "Inventar.db");
                    this.Database = Path.GetFileName(connectionInfo);
                    this.SqlConnectionString = this.ConnectStringToText(this.FullName);
                }

                this.OpenConnection();
            }
            catch (Exception ex) when (ex is FileNotFoundException)
            {
                string errorText = ex.Message;
                throw new FileNotFoundException($"Die Datenbankdatei '{connectionInfo}' ist nicht vorhanden!", ex);
            }
            catch (Exception ex) when (ex is SQLiteException)
            {
                string errorText = ex.Message;
                throw new FileNotFoundException($"Es ist ein Problem mit der Datenbank '{connectionInfo}' aufgetreten!",ex);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw new FileNotFoundException($"Es ist ein Problem mit der Datenbank '{connectionInfo}' aufgetreten!", ex);
            }
        }

        private string ConnectStringToText(string databasePath)
        {
            SQLiteConnectionStringBuilder conString = new SQLiteConnectionStringBuilder();
            conString.DataSource = databasePath;
            conString.DefaultTimeout = 30;
            conString.SyncMode = SynchronizationModes.Off;
            conString.JournalMode = SQLiteJournalModeEnum.Memory;
            conString.PageSize = 65536;
            conString.CacheSize = 16777216;
            conString.FailIfMissing = false;
            conString.ReadOnly = false;
            conString.Version = 3;
            conString.UseUTF16Encoding = true;

            return conString.ToString();
        }

        #region Implement Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing == true)
                {
                    if (this.Connection != null)
                    {
                        this.FullName = null;
                        this.Database = null;
                        this.SqlConnectionString = null;
                        this.IsOpen = false;
                        this.Connection = null;
                    }
                }
            }

            this.classIsDisposed = true;
        }

        #endregion Implement Dispose
    }
}
