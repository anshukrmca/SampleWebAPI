using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.Infrastructure.Repository;

namespace WFM.Infrastructure.Interfaces
{
    public interface ICommonRepository
    {
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, object parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure);
        //bool Execute(object parms, Core.Entities.HeaderSaveVO headerDetailConfig);
        //bool Execute(object parms, Core.Entities.HeaderSaveVO headerDetailConfig, IDbTransaction dbTransaction);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, DynamicParameters parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure);
        List<T> Insert<T>(string sp, List<T> listParms, CommandType commandType = CommandType.StoredProcedure);
        List<T> Insert<T>(string sp, List<T> listParms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure);
        T InsertWithoutTransaction<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure);
        T GetMultipleResultSets<T>(string sp, DynamicParameters parms, Mapper<T> mapper, CommandType commandType = CommandType.StoredProcedure);
        DataSet GetDataSetFromSP(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        
    }
}
