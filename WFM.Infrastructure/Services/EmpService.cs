using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.Infrastructure.Interfaces;
using WFM.Infrastructure;
using WFM.Core.Entities;
using WFM.Infrastructure.Repository;
using WFM.Core.DTO;

namespace WFM.Infrastructure.Services
{
    public class EmpService : IEmpService
    {
        private readonly ICommonRepository _commonRepository;
        readonly string ls_tenantConnection;

        public EmpService(string connection = "DefaultConnection")
        {
            _commonRepository = new CommonRepository();
            ls_tenantConnection = connection;
        }

        public async Task<List<Emp>> GetAttendancesList()
        {
            List<Emp> result = null;
            string query = "ListStudentList";
            DynamicParameters parameters = new DynamicParameters();
            result = await Task.FromResult(_commonRepository.GetAll<Emp>(query, parameters, commandType: CommandType.StoredProcedure));
            return result;
        }

        public async Task<List<EmpDTO>> GetAttendancesList(string AutoCode)
        {
            List<EmpDTO> result = null;
            string query = "DetailStudentDetail";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AutoCode", AutoCode, DbType.String, ParameterDirection.Input);
            result = await Task.FromResult(_commonRepository.GetAll<EmpDTO>(query, parameters, commandType: CommandType.StoredProcedure));
            return result;
        }

        public async Task<ErrStatus> GetStudentAdd(Emp_StudentReg obj)
        {
            ErrStatus result = new ErrStatus();
            try
            {
                string query = "Emp_StudentRegAdd";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name ", obj.Name, DbType.String, ParameterDirection.Input);
                parameters.Add("@FathersName ", obj.FathersName, DbType.String, ParameterDirection.Input);
                parameters.Add("@MothersName ", obj.MothersName, DbType.String, ParameterDirection.Input);
                parameters.Add("@Email ", obj.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("@Address ", obj.Address, DbType.String, ParameterDirection.Input);
                parameters.Add("@City ", obj.City, DbType.String, ParameterDirection.Input);
                parameters.Add("@State ", obj.State, DbType.String, ParameterDirection.Input);
                parameters.Add("@Country ", obj.Country, DbType.String, ParameterDirection.Input);
                parameters.Add("@ZipCode ", obj.ZipCode, DbType.String, ParameterDirection.Input);
                await Task.FromResult(_commonRepository.Execute(query, parameters, commandType: CommandType.StoredProcedure));
                result.ErrFlag = false;
                result.ErrMessage = "";
            }
            catch (Exception e)
            {
                result.ErrFlag = true;
                result.ErrMessage = e.Message;
            }
            return result;
        }

        public async Task<List<Emp_StudentReg>> GetStudentList()
        {
            List<Emp_StudentReg> result = null;
            string query = "Emp_StudentRegList";
            DynamicParameters parameters = new DynamicParameters();
            result = await Task.FromResult(_commonRepository.GetAll<Emp_StudentReg>(query, parameters, commandType: CommandType.StoredProcedure));
            return result;
        }

        public async Task<ErrStatus> GetStudentDelete(int studentRegID)
        {
            ErrStatus result = new ErrStatus();
            try
            {
                string query = "Emp_StudentRegDelete";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@studentRegID ", studentRegID, DbType.String, ParameterDirection.Input);
                await Task.FromResult(_commonRepository.Execute(query, parameters, commandType: CommandType.StoredProcedure));
                result.ErrFlag = false;
                result.ErrMessage = "";
            }
            catch (Exception e)
            {
                result.ErrFlag = true;
                result.ErrMessage = e.Message;
            }
            return result;
        }

        public async Task<ErrStatus> GetStudentEdit(Emp_StudentReg obj)
        {
            ErrStatus result = new ErrStatus();
            try
            {
                string query = "Emp_StudentRegEdit";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentRegID ", obj.StudentRegID, DbType.String, ParameterDirection.Input);
                parameters.Add("@Name ", obj.Name, DbType.String, ParameterDirection.Input);
                parameters.Add("@FathersName ", obj.FathersName, DbType.String, ParameterDirection.Input);
                parameters.Add("@MothersName ", obj.MothersName, DbType.String, ParameterDirection.Input);
                parameters.Add("@Email ", obj.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("@Address ", obj.Address, DbType.String, ParameterDirection.Input);
                parameters.Add("@City ", obj.City, DbType.String, ParameterDirection.Input);
                parameters.Add("@State ", obj.State, DbType.String, ParameterDirection.Input);
                parameters.Add("@Country ", obj.Country, DbType.String, ParameterDirection.Input);
                parameters.Add("@ZipCode ", obj.ZipCode, DbType.String, ParameterDirection.Input);
                await Task.FromResult(_commonRepository.Execute(query, parameters, commandType: CommandType.StoredProcedure));
                result.ErrFlag = false;
                result.ErrMessage = "";
            }
            catch (Exception e)
            {
                result.ErrFlag = true;
                result.ErrMessage = e.Message;
            }
            return result;
        }

        public async Task<List<Emp_StudentReg>> GetStudentWithID(int studentRegID)
        {
            List<Emp_StudentReg> result = null;
            string query = "Emp_StudentRegDetail";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@studentRegID", studentRegID, DbType.String, ParameterDirection.Input);
            result = await Task.FromResult(_commonRepository.GetAll<Emp_StudentReg>(query, parameters, commandType: CommandType.StoredProcedure));
            return result;
        }


    }
    public class ErrStatus
    {
        public bool ErrFlag { get; set; }
        public string ErrMessage { get; set; }
    }
}
//