//-----------------------------------------------------------------------
// <copyright file="AttachmentRepository.cs" company="Lifeprojects.de">
//     Class: AttachmentRepository
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

    public class AttachmentRepository<TEntity> : RepositoryBase<TEntity>
    {
        public AttachmentRepository()
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

        public byte[] GetFirst()
        {
            byte[] result = null;

            try
            {
                result = base.Connection.RecordSet<byte[]>($"select content from {this.Tablename} LIMIT 1").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public IEnumerable<TEntity> Select()
        {
            List<TEntity> result = null;

            try
            {
                result = base.Connection.RecordSet<List<TEntity>>($"select * from {this.Tablename}").Get().Result;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        public void Add(Attachments entity)
        {
            try
            {
                SQLGenerator<Attachments> insert = new SQLGenerator<Attachments>(entity);
                string resultSql = insert.Insert().ToSql();
                Dictionary<string, object> parameterCollection = new Dictionary<string, object>();
                parameterCollection.Add("@Content", entity.Content);
                base.Connection.CmdExecuteNonQuery(resultSql, parameterCollection);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void Update(Attachments entity)
        {
            try
            {
                SQLGenerator<Attachments> updateInv = new SQLGenerator<Attachments>(entity);
                string resultSql = updateInv.Update().Where(w => w.Id, SqlComparison.Equals, entity.Id).ToSql();
                Dictionary<string, object> parameterCollection = new Dictionary<string, object>();
                parameterCollection.Add("@Content", entity.Content);
                base.Connection.CmdExecuteNonQuery(resultSql, parameterCollection);
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
