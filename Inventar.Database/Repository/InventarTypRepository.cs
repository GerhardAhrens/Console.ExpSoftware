//-----------------------------------------------------------------------
// <copyright file="InventarTypRepository.cs" company="Lifeprojects.de">
//     Class: InventarTypRepository
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.06.2025</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.Database.Repository
{
    using System;
    using System.Data.Common;
    using System.Data.SQLite;

    using Inventar.DatabaseCore;
    using Inventar.Generator;
    using Inventar.Model;

    public class InventarTypRepository<TEntity> : RepositoryBase<TEntity>
    {
        public InventarTypRepository()
        {
           this.Tablename = base.DataTableName();
        }

        private string Tablename { get; set; }

        public int Count()
        {
            int result = 0;

            try
            {
                result = new RecordSet<int>(base.Connection, $"select count(*) from {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public IEnumerable<InventarTyp> Select()
        {
            List<InventarTyp> result = null;

            try
            {
                //Dictionary<int, string> invDict = base.Connection.RecordSet<Dictionary<int, string>>($"SELECT typ,name FROM {this.Tablename}").Get().Result;
                List<string> invListString = base.Connection.RecordSet<List<string>>($"SELECT name FROM {this.Tablename}").Get().Result;
                List<int> invListInt = base.Connection.RecordSet<List<int>>($"SELECT typ FROM {this.Tablename}").Get().Result;
                List<Guid> invListGuid = base.Connection.RecordSet<List<Guid>>($"SELECT id FROM {this.Tablename}").Get().Result;

                result = base.Connection.RecordSet<List<InventarTyp>>($"SELECT * FROM {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public void Add(InventarTyp entity)
        {
            try
            {
                SQLGenerator<InventarTyp> insertInvTyp = new SQLGenerator<InventarTyp>(entity);
                string resultSql = insertInvTyp.Insert().ToSql();
                base.Connection.CmdExecuteNonQuery(resultSql);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void Update(InventarTyp entity)
        {
            try
            {
                SQLGenerator<InventarTyp> updateInv = new SQLGenerator<InventarTyp>(entity);
                string resultSql = updateInv.Update().Where(w => w.Id, SQLComparison.Equals, entity.Id).ToSql();
                base.Connection.CmdExecuteNonQuery(resultSql);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public int Delete(Attachments entity)
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
