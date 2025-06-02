//-----------------------------------------------------------------------
// <copyright file="InventarRepository.cs" company="Lifeprojects.de">
//     Class: InventarRepository
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.06.2025 11:54:41</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.Database.Repository
{
    using System;
    using System.Data.SQLite;

    using Inventar.DatabaseCore;
    using Inventar.Generator;
    using Inventar.Model;

    public class InventarRepository<TEntity> : RepositoryBase<TEntity>
    {
        public InventarRepository()
        {
           this.Tablename = base.DataTableName();
        }

        private string Tablename { get; set; }

        public int Count()
        {
            int result = 0;

            try
            {
                result = new RecordSet<int>(base.Connection, $"select count(*) from {this.Tablename}", RecordSetResult.Scalar).Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public IEnumerable<Inventars> Select()
        {
            List<Inventars> result = null;

            try
            {
                result = new RecordSet<List<Inventars>>(base.Connection, $"select * from {this.Tablename}", RecordSetResult.ListOfT).Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public void Add(TEntity entity)
        {
            try
            {
                SQLGenerator<TEntity> insertInv = new SQLGenerator<TEntity>(entity);
                string resultSql = insertInv.Insert().ToSql();
                base.Connection.CmdExecuteNonQuery(resultSql);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void Update(Inventars entity)
        {
            try
            {
                SQLGenerator<Inventars> updateInv = new SQLGenerator<Inventars>(entity);
                string resultSql = updateInv.Update().Where(w => w.Id, SQLComparison.Equals, entity.Id).ToSql();
                base.Connection.CmdExecuteNonQuery(resultSql);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void Delete(Inventars entity)
        {
            try
            {
                SQLGenerator<Inventars> deleteInv = new SQLGenerator<Inventars>(entity);
                string resultSql = deleteInv.Delete(d => d.Id).ToSql();
                int deleteCount = base.Connection.CmdExecuteNonQuery(resultSql);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public int DeleteAll()
        {
            int result = 0;

            try
            {
                result = new RecordSet<int>(base.Connection, $"DELETE FROM {this.Tablename}", RecordSetResult.ExecuteNonQuery).Set().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }
    }
}
