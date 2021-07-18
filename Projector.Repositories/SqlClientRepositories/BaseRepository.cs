using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Repositories.SqlClientRepositories
{
    public abstract class BaseRepository
    {
        protected string _connString;

        public BaseRepository(string connecionString)
        { 
            _connString = connecionString;
        }

       
    }
}