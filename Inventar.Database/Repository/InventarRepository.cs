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
                result = base.Connection.RecordSet<int>($"select count(*) from {this.Tablename}").Get().Result;
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
                result = base.Connection.RecordSet<List<Inventars>>($"select * from {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public IEnumerable<Inventars> SelectByInventarTyp(int inventarTyp)
        {
            List<Inventars> result = null;

            try
            {
                Dictionary<string, object> parameterCollection = new Dictionary<string, object>();
                parameterCollection.Add("@InventarTyp", inventarTyp);

                result = base.Connection.RecordSet<List<Inventars>>($"select * from {this.Tablename} WHERE (InventarTyp = @InventarTyp)", parameterCollection).Get().Result;
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
                string resultSql = updateInv.Update().Where(w => w.Id, SqlComparison.Equals, entity.Id).ToSql();
                base.Connection.CmdExecuteNonQuery(resultSql);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public int Delete(Inventars entity)
        {
            int result = 0;

            try
            {
                Dictionary<string, object> parameterCollection = new Dictionary<string, object>();
                parameterCollection.Add("@Id", entity.Id.ToString());
                result = base.Connection.RecordSet<int>($"DELETE FROM {this.Tablename} WHERE (Id = @Id)", parameterCollection).Set().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public int DeleteAll()
        {
            int result = 0;

            try
            {
                result = base.Connection.RecordSet<int>($"DELETE FROM {this.Tablename}").Set().Result;
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
