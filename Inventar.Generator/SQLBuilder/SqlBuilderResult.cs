//-----------------------------------------------------------------------
// <copyright file="SqlBuilderResult.cs" company="Lifeprojects.de">
//     Class: SqlBuilderResult
//     Copyright � Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.04.2024 12:02:41</date>
//
// <summary>
// Klasse zur R�ckgabe 
// </summary>
//-----------------------------------------------------------------------

namespace Mainova.Tools.Data
{
    using System;
    using System.Data.SQLite;

    public sealed class SqlBuilderResult : Tuple<string, SQLiteParameter[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBuilderResult"/> class.
        /// </summary>
        public SqlBuilderResult(string sql, SQLiteParameter[] oparam) : base(sql, oparam)
        {
            this.SqlStatement = sql;
            this.Parameter = oparam;
        }

        public string SqlStatement { get; private set; }

        public SQLiteParameter[] Parameter { get; private set; }
    }
}
