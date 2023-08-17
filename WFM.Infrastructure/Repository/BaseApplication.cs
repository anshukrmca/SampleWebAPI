using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM.Infrastructure.Repository
{
    public abstract class BaseApplication
    {
        private string Connectionstring = "DefaultConnection";

        public BaseApplication()
        {
            this.Connectionstring = System.Environment.GetEnvironmentVariable(Connectionstring);

        }

        protected IDbConnection GetSqlconnection
        {
            get
            {
                return new SqlConnection(Connectionstring);
            }


        }
        public string GetTenantConnectionstring
        {
            get
            {
                return Connectionstring;
            }
        }
        public string SetConnectionstring
        {
            set
            {
                //System.Environment.GetEnvironmentVariable(value) later get from this;
                this.Connectionstring = value;

            }


        }
    }
}
