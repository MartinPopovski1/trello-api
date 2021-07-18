using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Projector.Repositories.SqlClientRepositories
{
    public interface IGenericRepository<T, TKey>
    {
        T Add(T entity);
        void Edit(T entity);
        void Delete(TKey key);
        T Get(TKey key);
        IEnumerable<T> GetAll();        
    }
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseModel
    {
        private string _connString;
        public GenericRepository(string connectionString) 
        {
            _connString = connectionString;
        }
        public T Add(T entity)
        {
            using(var sqlConnection = new SqlConnection(_connString))
            {
                sqlConnection.Open();

                string insertSql = "INSERT INTO PROJECT([Id],'Name','Deleted' ...) VALUES ( , , , , , ,)";
                StringBuilder stringBuilder = new StringBuilder(insertSql);

                var entityType = typeof(T);
                stringBuilder.Append($" {entityType.Name}(");
      
                List<Tuple<object,Type>> values = new List<Tuple<object,Type>>();
                
                entity.GetType().GetProperties().ToList().ForEach(p =>
                {
                    var value = p.GetValue(entity);
                    stringBuilder.Append($"[{p.Name}],");

                    values.Add(new Tuple<object, Type>(value, p.GetType()));
                });

                stringBuilder.Remove(stringBuilder.Length - 1, stringBuilder.Length);
                stringBuilder.Append(") VALUES(");

                values.ForEach(value =>
                {
                    if(value.Item2 == typeof(DateTime))
                    {
                        
                    }
                    else if(value.Item2 == typeof(Guid) || value.Item2 == typeof(string))
                    {
                        stringBuilder.Append($"'{value.Item1.ToString()}'");                  
                    }
                });


                sqlConnection.Close();
            }
            return null;
        }

        public void Delete(TKey key)
        {
            using (var sqlConnection = new SqlConnection(_connString))
            {
                sqlConnection.Open();

                string insertSql = "DELETE FROM ";
                StringBuilder stringBuilder = new StringBuilder(insertSql);

                var entityType = typeof(T);
                stringBuilder.Append($" {entityType.Name}(");

                stringBuilder.Append("Where Id = ");
            }
        }

        public void Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public T Get(TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}