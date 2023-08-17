using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using static Dapper.SqlMapper;
using System.Data.SqlClient;
using WFM.Infrastructure.Interfaces;

namespace WFM.Infrastructure.Repository
{
    public delegate T Mapper<T>(GridReader gridReader);
    public class CommonRepository : BaseApplication, ICommonRepository
    {
        public void Dispose()
        {
        }

        public DataSet GetDataSetFromSP(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = GetSqlconnection)
            {
                DataSet ds = new DataSet();
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    IDataReader dr = db.ExecuteReader(sp, parms, null, commandType: commandType);
                    while (!dr.IsClosed)
                        ds.Tables.Add().Load(dr);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
                return ds;
            }
        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            int result;
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using var tran = db.BeginTransaction();
                    try
                    {
                        result = db.Execute(sp, parms, commandType: commandType, transaction: tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

                return result;
            }
            //using (IDbConnection db = GetSqlconnection)
            //{
            //    return GetSqlconnection.Execute(sp, parms, commandType: commandType);
            //}
        }

        public int Execute(string sp, DynamicParameters parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure)
        {
            int result;
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    try
                    {
                        result = db.Execute(sp, parms, commandType: commandType, transaction: dbTransaction);
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

                return result;
            }
        }


        public int Execute(string sp, object parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //using (IDbConnection db = GetSqlconnection)
            //{
            //    return GetSqlconnection.Execute(sp, parms, commandType: commandType);
            //}
            int result;
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using var tran = db.BeginTransaction();
                    try
                    {
                        result = db.Execute(sp, parms, commandType: commandType, transaction: tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

                return result;
            }
        }

        public int Execute(string sp, object parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure)
        {
            int result;
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    try
                    {
                        result = db.Execute(sp, parms, commandType: commandType, transaction: dbTransaction);
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

                return result;
            }
        }

       
        object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }



        DynamicParameters CleanDBParam(object param)
        {
            DynamicParameters parameters = new DynamicParameters();
            foreach (PropertyInfo property in param.GetType().GetProperties().ToList().Where(i => !((PropertyInfo)i).PropertyType.IsGenericType || ((PropertyInfo)i).PropertyType.IsValueType))
            {
                parameters.Add("@" + property.Name, property.GetValue(param, null));
            }
            return parameters;
        }

        object GetIdValue(dynamic row, string spName)
        {
            object id = ((IDictionary<string, object>)row)["Id"];
            if (id == null || id == GetDefaultValue(id.GetType()))
            {
                throw new Exception("In '" + spName + ", Stored Procedure - Not returning Id value.");
            }
            return id;
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection db = GetSqlconnection)
            {
                return GetSqlconnection.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
            }
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using (IDbConnection db = GetSqlconnection)
            {
                return db.Query<T>(sp, parms, commandType: commandType).ToList();
            }
        }
        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            return Insert<T>(sp, parms, commandType);
        }

        public T Update<T>(string sp, DynamicParameters parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure)
        {
            return Insert<T>(sp, parms, dbTransaction, commandType);
        }

        public List<T> Insert<T>(string sp, List<T> listParms, CommandType commandType = CommandType.StoredProcedure)
        {
            List<T> returnValues = new List<T>();
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using var tran = db.BeginTransaction();
                    foreach (T item in listParms)
                    {
                        T result = default(T);
                        try
                        {
                            var re = GetLastResultSet(db.QueryMultiple(sp, CleanDBParam(item), tran, commandType: commandType));
                            if (re != null)
                            {
                                IDictionary<string, object> firstRow = re.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                                if (typeof(T).IsValueType || typeof(T).Name == "String")
                                {
                                    result = (T)firstRow.Values.FirstOrDefault();
                                }
                                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re));
                                }
                                else
                                {
                                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re.FirstOrDefault()));
                                }
                                LogMessage(firstRow);
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                        returnValues.Add(result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
            }
            return returnValues;
        }

        public List<T> Insert<T>(string sp, List<T> listParms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure)
        {
            List<T> returnValues = new List<T>();
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    foreach (T item in listParms)
                    {
                        T result = default(T);
                        try
                        {
                            var re = GetLastResultSet(db.QueryMultiple(sp, CleanDBParam(item), dbTransaction, commandType: commandType));
                            if (re != null)
                            {
                                IDictionary<string, object> firstRow = re.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                                if (typeof(T).IsValueType || typeof(T).Name == "String")
                                {
                                    result = (T)firstRow.Values.FirstOrDefault();
                                }
                                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re));
                                }
                                else
                                {
                                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re.FirstOrDefault()));
                                }
                                LogMessage(firstRow);
                            }
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw ex;
                        }
                        returnValues.Add(result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
            }
            return returnValues;
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result = default(T);
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    using var tran = db.BeginTransaction();
                    try
                    {
                        var re = GetLastResultSet(db.QueryMultiple(sp, parms, tran, commandType: commandType));
                        //var re = db.Query(sp, parms, commandType: commandType, transaction: tran);
                        if (re != null)
                        {
                            IDictionary<string, object> firstRow = re.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                            if (typeof(T).IsValueType || typeof(T).Name == "String")
                            {
                                result = (T)firstRow.Values.FirstOrDefault();
                            }
                            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                            {
                                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re));
                            }
                            else
                            {
                                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re.FirstOrDefault()));
                            }
                            LogMessage(firstRow);
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

                return result;
            }
        }

        public T Insert<T>(string sp, DynamicParameters parms, IDbTransaction dbTransaction, CommandType commandType = CommandType.StoredProcedure)
        {
            T result = default(T);
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();

                    try
                    {
                        var re = GetLastResultSet(db.QueryMultiple(sp, parms, dbTransaction, commandType: commandType));
                        //var re = db.Query(sp, parms, commandType: commandType, transaction: tran);
                        if (re != null)
                        {
                            IDictionary<string, object> firstRow = re.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                            if (typeof(T).IsValueType || typeof(T).Name == "String")
                            {
                                result = (T)firstRow.Values.FirstOrDefault();
                            }
                            else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                            {
                                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re));
                            }
                            else
                            {
                                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re.FirstOrDefault()));
                            }
                            LogMessage(firstRow);
                        }
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

                return result;
            }
        }

        public T InsertWithoutTransaction<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            T result = default(T);
            using (IDbConnection db = GetSqlconnection)
            {
                var re = GetLastResultSet(db.QueryMultiple(sp, parms, commandType: commandType));
                //var re = db.Query(sp, parms, commandType: commandType, transaction: tran);
                if (re != null)
                {
                    IDictionary<string, object> firstRow = re.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                    if (typeof(T).IsValueType || typeof(T).Name == "String")
                    {
                        result = (T)firstRow.Values.FirstOrDefault();
                    }
                    else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                    {
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re));
                    }
                    else
                    {
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(re.FirstOrDefault()));
                    }
                    LogMessage(firstRow);
                }
                return result;
            }
        }
        IEnumerable<dynamic> GetLastResultSet(GridReader reader)
        {
            IEnumerable<dynamic> f = reader.Read();
            while (!reader.IsConsumed)
            {
                LogMessage(f.Select(i => (IDictionary<string, object>)i).FirstOrDefault());
                f = reader.Read();
            }
            return f;
        }

        void LogMessage(IDictionary<string, object> firstRow)
        {
            if (firstRow != null && firstRow.ContainsKey("LogMessage"))
            {
                return;
            }
        }

        public T GetMultipleResultSets<T>(string sp, DynamicParameters parms, Mapper<T> mapper, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    GridReader reader = db.QueryMultiple(sp, parms, null, commandType: commandType);
                    result = mapper.Invoke(reader);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }

            }
            return result;
        }

        public void ExecuteMultipleCommands(List<SP_ExcuteDetail> excuteDetails)
        {
            using (IDbConnection db = GetSqlconnection)
            {
                try
                {
                    if (db.State == ConnectionState.Closed)
                        db.Open();
                    using var dbTransaction = db.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < excuteDetails.Count; i++)
                        {
                            SP_ExcuteDetail excuteDetail = excuteDetails[i];
                            if (excuteDetail.DependentResultNo.HasValue)
                            {
                                if (excuteDetail.DependentResultNo.Value >= i)
                                    throw new Exception(excuteDetail.SPName + " - Invaild Dependent Result Number");
                                SP_ExcuteDetail dependentDetail = excuteDetails[excuteDetail.DependentResultNo.Value];
                                if (dependentDetail.Result == null)
                                    throw new Exception("Dependent Result not available.");
                                if (dependentDetail.Result.Count() == 0)
                                    throw new Exception("Dependent Result not available.");
                                IDictionary<string, object> firstRow = dependentDetail.Result.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                                excuteDetail.Parameters.Add(dependentDetail.DependentParameterName, firstRow[dependentDetail.DependentResultProperty]);
                            }
                            excuteDetail.Execute(db, dbTransaction);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        throw ex;
                    }
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (db.State == ConnectionState.Open)
                        db.Close();
                }
            }
        }

    }

    public class SP_ExcuteDetail
    {
        public string SPName { get; set; }
        public DynamicParameters Parameters { get; set; }
        public CommandType CommandType { get; set; } = CommandType.StoredProcedure;
        public IEnumerable<dynamic> Result { get; set; }
        public string DependentParameterName { get; set; }
        public int? DependentResultNo { get; set; }
        public string DependentResultProperty { get; set; }
        public T ReturnObject<T>()
        {
            T result = default(T);
            if (Result != null)
            {
                if (typeof(T).IsValueType || typeof(T).Name == "String")
                {
                    IDictionary<string, object> firstRow = Result.Select(i => (IDictionary<string, object>)i).FirstOrDefault();
                    result = (T)firstRow.Values.FirstOrDefault();
                }
                else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(Result));
                }
                else
                {
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(Result.FirstOrDefault()));
                }
            }
            return result;
        }

        public void Execute(IDbConnection db, IDbTransaction transaction = null)
        {
            if (db == null)
                throw new ArgumentNullException("Db Connection is required.");
            if (transaction == null)
                throw new ArgumentNullException("DB Transaction is required.");
            if (db.State == ConnectionState.Closed)
                db.Open();
            Result = db.Query(SPName, Parameters, commandType: CommandType, transaction: transaction);
        }
    }
}
