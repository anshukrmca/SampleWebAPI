using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.Core.DTO;
using WFM.Core.Entities;
using WFM.Infrastructure.Services;

namespace WFM.Infrastructure.Interfaces
{
    public interface IEmpService : IService
    {
        public Task<List<Emp>> GetAttendancesList();
        public Task<List<EmpDTO>> GetAttendancesList(string Empid);
        public Task<ErrStatus> GetStudentAdd(Emp_StudentReg obj);
        public Task<List<Emp_StudentReg>> GetStudentList();
        public Task<ErrStatus> GetStudentDelete(int studentRegID);

        public Task<ErrStatus> GetStudentEdit(Emp_StudentReg obj);

        public Task<List<Emp_StudentReg>> GetStudentWithID(int studentRegID);
    }

}
