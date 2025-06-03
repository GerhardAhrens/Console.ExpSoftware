//-----------------------------------------------------------------------
// <copyright file="RecordSet.cs" company="Lifeprojects.de">
//     Class: RecordSet
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>112.05.2025</date>
//
// <summary>
// Klasse zur vereinfachten verwenden von SQL Anweisungen. Die Klasse RecordSet ermöglicht es, einen bestimmen Datentyp aus der Datenbank zurückzugeben.
// </summary>
// <example>
// int result = new RecordSet<int>(this.Connection, "select count(*) from Table", RecordSetResult.Scalar).Get().Result;
//
// DataRow row = new RecordSet<DataRow>(this.Connection, "select * from Table where id = 1", RecordSetResult.DataRow).Get().Result;
//
// DataTable result = new RecordSet<DataTable>(repository.GetConnection, "select * from Table", RecordSetResult.DataTable).Get().Result;
//
// ICollectionView result = new RecordSet<ICollectionView>(repository.GetConnection, sql, RecordSetResult.CollectionView).Get().Result;
// </example>
//-----------------------------------------------------------------------

namespace Inventar.Generator
{
    using System.ComponentModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    public class RecordSet<T> : IDisposable
    {
        private bool classIsDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordSet"/> class.
        /// </summary>
        /// <param name="connection">Connection Objekt zur Datenbank</param>
        /// <param name="sql">SQL Anweisung</param>
        /// <param name="resultTyp">Ergebnis Typ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RecordSet(SQLiteConnection connection, string sql)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("Das Connection-Objekt darf nicht 'null' sein");
            }

            if (string.IsNullOrEmpty(sql) == true)
            {
                throw new ArgumentNullException("Der String mit für die SQL-Anweisung darf nicht 'null' oder leer sein");
            }

            try
            {
                this.Connection = connection;
                this.SQL = sql;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
        }

        public T Result { get; private set; }

        public SQLiteConnection Connection { get; set; }

        public string SQL { get; set; }

        #region SET
        public RecordSet<T> Set()
        {
            try
            {
                if (this.CheckSetResultParameter(typeof(T)) == false)
                {
                    throw new ArgumentException($"Der Typ '{typeof(T).Name}' ist für das Schreiben des RecordSet nicht gültig.");
                }

                if (typeof(T).IsGenericType == false)
                {
                    this.Result = this.SetExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return this;
        }

        private T SetExecuteNonQuery()
        {
            object getAs = null;

            try
            {
                using (SQLiteCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.SQL;
                    int? result = cmd.ExecuteNonQuery();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }

            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        #endregion SET

        #region GET
        public RecordSet<T> Get()
        {
            try
            {
                if (this.CheckGetResultParameter(typeof(T)) == false)
                {
                    throw new ArgumentException($"Der Typ '{typeof(T).Name}' ist für die Rückgabe des RecordSet Result nicht gültig.");
                }

                if (typeof(T) == typeof(DataRow))
                {
                    this.Result = this.GetSingle();
                }
                else if (typeof(T) == typeof(ICollectionView))
                {
                    this.Result = this.GetCollectionView();
                }
                else if (typeof(T).IsGenericType == false && typeof(T).IsPrimitive == true && typeof(T).Namespace == "System")
                {
                    this.Result = this.GetScalar();
                }
                else if (typeof(T) == typeof(DataTable))
                {
                    this.Result = this.GetDataTable();
                }
                else if (typeof(T).IsGenericType == true && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    this.Result = this.GetListOfT();
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return this;
        }

        private T GetSingle()
        {
            object result = null;

            try
            {
                using (SQLiteCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.SQL;

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true && dr.VisibleFieldCount > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.TableName = this.ExtractTablename(this.SQL);
                            dt.Load(dr);
                            result = dt.Rows[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        private T GetCollectionView()
        {
            ICollectionView result;

            try
            {
                using (SQLiteCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.SQL;

                    DataTable dt = null;
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true && dr.VisibleFieldCount > 0)
                        {
                            dt = new DataTable();
                            dt.Load(dr);
                        }
                    }

                    result = CollectionViewSource.GetDefaultView(dt.Rows) as CollectionView;
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        private T GetScalar()
        {
            object getAs = null;

            try
            {
                using (SQLiteCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.SQL;
                    var result = cmd.ExecuteScalar();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }

            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        private T GetDataTable()
        {
            object result = null;

            try
            {
                using (SQLiteCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.SQL;

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            result = new DataTable();
                            ((DataTable)result).TableName = this.ExtractTablename(this.SQL);
                            ((DataTable)result).Load(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        private T GetListOfT()
        {
            T result = default;

            try
            {
                using (SQLiteCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = this.SQL;

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            /* Typ für List<T> erstellen */
                            Type typeCollection = typeof(T);
                            result = (T)Activator.CreateInstance(typeCollection);

                            while (dr.Read())
                            {
                                var columnCount = dr.FieldCount;

                                /* Typ für List-Content erstellen */
                                Type genericType = typeCollection.GetGenericArguments()[0];
                                var instance = Activator.CreateInstance(genericType);
                                if (instance != null)
                                {
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        string columnName = dr.GetName(i);
                                        object columnValue = dr[i];
                                        PropertyInfo itemProperty = instance.GetType().GetProperty(columnName);

                                        if (itemProperty == null && itemProperty.CanWrite == false)
                                        {
                                            continue;
                                        }

                                        if (itemProperty.PropertyType == typeof(Guid))
                                        {
                                            itemProperty.SetValue(instance, new Guid(columnValue.ToString()), null);
                                        }
                                        else if (itemProperty.PropertyType == typeof(int))
                                        {
                                            itemProperty.SetValue(instance, dr.GetInt32(i), null);
                                        }
                                        else if (itemProperty.PropertyType == typeof(long))
                                        {
                                            itemProperty.SetValue(instance, Convert.ToInt64(columnValue), null);
                                        }
                                        else if (itemProperty.PropertyType == typeof(DateTime))
                                        {
                                            itemProperty.SetValue(instance, dr.GetDateTime(i), null);
                                        }
                                        else if (itemProperty.PropertyType == typeof(bool))
                                        {
                                            itemProperty.SetValue(instance, dr.GetBoolean(i), null);
                                        }
                                        else if (itemProperty.PropertyType == typeof(byte[]))
                                        {
                                            byte[] byteArray = (byte[])dr.GetValue(i);
                                            itemProperty.SetValue(instance, byteArray, null);
                                        }
                                        else
                                        {
                                            itemProperty.SetValue(instance, columnValue, null);
                                        }
                                    }
                                }

                                /* Add Methode mit Content per Invoke erstellen */
                                MethodInfo method = typeCollection.GetMethod("Add");
                                method.Invoke(result, new object[] { instance });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }        
        #endregion GET

        private string ExtractTablename(string sql)
        {
            try
            {
                List<string> tables = new List<string>();

                Regex r = new Regex(@"(from|join|into)\s+(?<table>\S+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match ma = r.Match(sql);
                while (ma.Success)
                {
                    tables.Add(ma.Groups["table"].Value);
                    ma = ma.NextMatch();
                }

                return tables.FirstOrDefault().ToUpper();
            }
            catch (Exception)
            {
                return $"TAB{DateTime.Now.ToString("yyyyMMdd")}";
            }
        }

        private bool CheckSetResultParameter(Type type)
        {
            bool result = false;

            if (type.Name == typeof(int).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(long).Name)
            {
                result = true;
            }

            return result;
        }

        private bool CheckGetResultParameter(Type type)
        {
            bool result = false;

            if (type.Name== typeof(DataRow).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(DataTable).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(ICollectionView).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(List<>).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(string).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(DateTime).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(bool).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(int).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(long).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(Single).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(decimal).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(float).Name)
            {
                result = true;
            }

            return result;
        }

        #region Dispose Function
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                    this.Result = default;
                    this.Connection = null;
                    this.SQL = null;
                }
            }

            this.classIsDisposed = true;
        }
        #endregion Dispose Function
    }
}
