//-----------------------------------------------------------------------
// <copyright file="SqlBuilderContext.cs" company="Lifeprojects.de">
//     Class: SqlBuilderContext
//     Copyright � Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>17.04.2024 15:38:15</date>
//
// <summary>
// Klasse zur Erstellung von SQL Anweisungen und die dazu passende SQLiteParameter
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    //[DebuggerNonUserCode]
    //[DebuggerStepThrough]
    public sealed class SqlBuilderContext : IDisposable
    {
        private bool classIsDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBuilderContext"/> class.
        /// </summary>
        public SqlBuilderContext(DataRow currentDataRow, string dataTableName)
        {
            this.CurrentDataRow = currentDataRow;
            this.TableName = dataTableName;
        }

        public SqlBuilderContext(DataRow currentDataRow)
        {
            this.CurrentDataRow = currentDataRow;
        }

        public SqlBuilderContext()
        {
            this.CurrentDataRow = null;
        }

        public DataRow CurrentDataRow { get; set; }

        public string CurrentUser { get; set; } = Environment.UserName;
        public string[] KeyColumn { get; set; } = { "Id" };
        public string CreateByColumn { get; set; } = "CreatedBy";
        public string CreateOnColumn { get; set; } = "CreatedOn";
        public string ModifyByColumn { get; set; } = "ModifiedBy";
        public string ModifyOnColumn { get; set; } = "ModifiedOn";

        public string TableName { get; set; }

        /// <summary>
        /// Erstellen einer SQL Anweisung auf Basis einer DataRow
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// SqlBuilderContext ctx = new SqlBuilderContext(dr);
        /// (string, SQLiteParameter[]) sql = ctx.GetInsert();
        /// </example>
        public (string, SQLiteParameter[]) GetInsert()
        {
            StringBuilder sb = null;
            SQLiteParameter[] prm = null;
            string tableName = string.Empty;
            if (this.CurrentDataRow != null)
            {
                sb = new StringBuilder();
                List<DataColumn> columnNames = new List<DataColumn>();
                List<string> columnParamNames = new List<string>();
                DataColumnCollection columns = this.CurrentDataRow.Table.Columns;
                if (string.IsNullOrEmpty(this.TableName) == true)
                {
                    tableName = this.CurrentDataRow.Table.TableName;
                }
                else
                {
                    tableName = this.TableName;
                }

                if (string.IsNullOrEmpty(tableName) == true)
                {
                    throw new ArgumentException("Name f�r DataTable ist nicht festgelegt");
                }

                foreach (DataColumn column in columns)
                {
                    if (column.ColumnName.ToLower() == this.KeyColumn.First().ToLower())
                    {
                        columnNames.Add(column);
                        columnParamNames.Add($":{column.ColumnName}");
                    }
                    else if (column.ColumnName.ToLower() == this.CreateByColumn.ToLower())
                    {
                        columnNames.Add(column);
                        columnParamNames.Add($":{column.ColumnName}");
                    }
                    else if (column.ColumnName.ToLower() == this.CreateOnColumn.ToLower())
                    {
                        columnNames.Add(column);
                        columnParamNames.Add($":{column.ColumnName}");
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyByColumn.ToLower())
                    {
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyOnColumn.ToLower())
                    {
                    }
                    else
                    {
                        columnNames.Add(column);
                        columnParamNames.Add($":{column.ColumnName}");
                    }
                }

                string names = string.Join(',', columnNames.Select(s => s.ColumnName));
                string paramNames = string.Join(',', columnParamNames);
                sb.Append($"INSERT INTO {tableName}").Append(" (");
                sb.Append(names).Append(")");
                sb.Append(" VALUES ").Append("(");
                sb.Append(paramNames).Append(")");

                /* Oracle Parameter */
                prm = new SQLiteParameter[columnNames.Count];
                int step = 0;
                foreach (DataColumn column in columnNames)
                {
                    if (column.ColumnName.ToLower() == this.CreateByColumn.ToLower())
                    {
                        prm[step] = new SQLiteParameter(column.ColumnName, this.CurrentUser);
                    }
                    else if (column.ColumnName.ToLower() == this.CreateOnColumn.ToLower())
                    {
                        prm[step] = new SQLiteParameter(column.ColumnName, DateTime.Now);
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyByColumn.ToLower())
                    {
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyOnColumn.ToLower())
                    {
                    }
                    else
                    {
                        prm[step] = new SQLiteParameter(column.ColumnName, this.CurrentDataRow[column]);
                    }

                    step++;
                }
            }

            return (sb.ToString(),prm);
        }

        public (string, SQLiteParameter[]) GetUpdate()
        {
            StringBuilder sb = null;
            SQLiteParameter[] prm = null;
            string tableName = string.Empty;

            if (this.CurrentDataRow != null)
            {
                sb = new StringBuilder();
                List<DataColumn> columnNames = new List<DataColumn>();
                List<string> columnParamNames = new List<string>();
                DataColumnCollection columns = this.CurrentDataRow.Table.Columns;
                if (string.IsNullOrEmpty(this.TableName) == true)
                {
                    tableName = this.CurrentDataRow.Table.TableName;
                }
                else
                {
                    tableName = this.TableName;
                }

                if (string.IsNullOrEmpty(tableName) == true)
                {
                    throw new ArgumentException("Name f�r DataTable ist nicht festgelegt");
                }

                sb.Append($"UPDATE {tableName} SET ");
                foreach (DataColumn column in columns)
                {
                    if (column.ColumnName.ToLower() == this.KeyColumn.First().ToLower())
                    {
                        columnNames.Add(column);
                    }
                    else if (column.ColumnName.ToLower() == this.CreateByColumn.ToLower())
                    {
                    }
                    else if (column.ColumnName.ToLower() == this.CreateOnColumn.ToLower())
                    {
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyByColumn.ToLower())
                    {
                        columnNames.Add(column);
                        sb.Append($"{column} = :{column}, ");
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyOnColumn.ToLower())
                    {
                        columnNames.Add(column);
                        sb.Append($"{column} = :{column}, ");
                    }
                    else
                    {
                        columnNames.Add(column);
                        sb.Append ($"{column} = :{column}, ");
                    }
                }

                sb.Remove(sb.ToString().Trim().Length-1,1);
                sb.Append($" WHERE {this.KeyColumn.First()} = :{this.KeyColumn.First()}");

                /* Oracle Parameter */
                prm = new SQLiteParameter[columnNames.Count];
                int step = 0;
                foreach (DataColumn column in columnNames)
                {
                    if (column.ColumnName.ToLower() == this.CreateByColumn.ToLower())
                    {
                    }
                    else if (column.ColumnName.ToLower() == this.CreateOnColumn.ToLower())
                    {
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyByColumn.ToLower())
                    {
                        prm[step] = new SQLiteParameter(column.ColumnName, this.CurrentUser);
                    }
                    else if (column.ColumnName.ToLower() == this.ModifyOnColumn.ToLower())
                    {
                        prm[step] = new SQLiteParameter(column.ColumnName, DateTime.Now);
                    }
                    else
                    {
                        prm[step] = new SQLiteParameter(column.ColumnName, this.CurrentDataRow[column]);
                    }

                    step++;
                }
            }

            return (sb.ToString(), prm);
        }

        public (string, SQLiteParameter[]) GetDelete()
        {
            StringBuilder sb = null;
            SQLiteParameter[] prm = null;
            string tableName = string.Empty;

            if (this.CurrentDataRow != null)
            {
                sb = new StringBuilder();
                List<DataColumn> columnNames = new List<DataColumn>();
                DataColumnCollection columns = this.CurrentDataRow.Table.Columns;
                if (string.IsNullOrEmpty(this.TableName) == true)
                {
                    tableName = this.CurrentDataRow.Table.TableName;
                }
                else
                {
                    tableName = this.TableName;
                }

                if (string.IsNullOrEmpty(tableName) == true)
                {
                    throw new ArgumentException("Name f�r DataTable ist nicht festgelegt");
                }

                sb.Append($"DELETE FROM {tableName}");
                sb.Append($" WHERE {this.KeyColumn.First()} = :{this.KeyColumn.First()}");

                /* SQLite Parameter */
                prm = new SQLiteParameter[1];
                prm[0] = new SQLiteParameter(this.KeyColumn.First(), this.CurrentDataRow[this.KeyColumn.First()]);
            }

            return (sb.ToString(), prm);
        }

        #region Dispose Function
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                }
            }

            this.classIsDisposed = true;
        }
        #endregion Dispose Function
    }
}
