using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WFM.Core.DTO;
using WFM.Core.Entities;
using WFM.Infrastructure.Interfaces;
using WFM.Infrastructure.Services;

namespace SampleWebAPI.Controllers
{

    [Route("api/wfh/[controller]")]
    [ApiController]
    public class EmpController : Controller
    {
        readonly IConfiguration _configuration;
        public EmpController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        [Consumes("application/json")]
        [Produces("application/json")]


        [HttpPost("getemplist")]
        public Task<List<Emp>> GetAttendancesList()
        {
            return ServiceManager.GetService<IEmpService>().GetAttendancesList();

        }

        [HttpPost("getemplistwithid")]
        public Task<List<EmpDTO>> GetAttendancesListt(string empid)
        {
            return ServiceManager.GetService<IEmpService>().GetAttendancesList(empid);
             
        }

        [HttpPost("StudentAdd")]
        public Task<ErrStatus> GetStudentAdd(Emp_StudentReg obj)
        {
            return ServiceManager.GetService<IEmpService>().GetStudentAdd(obj);

        }

        [HttpGet("Studentlist")]
        public Task<List<Emp_StudentReg>> GetStudentList()
        {
            return ServiceManager.GetService<IEmpService>().GetStudentList();

        }

        [HttpDelete("StudentDelete")]
        public Task<ErrStatus> GetStudentDelete(int studentRegID)
        {
            return ServiceManager.GetService<IEmpService>().GetStudentDelete(studentRegID);

        }

        [HttpPost("StudentEditwithID")]
        public Task<ErrStatus> GetStudentEdit(Emp_StudentReg obj)
        {
            return ServiceManager.GetService<IEmpService>().GetStudentEdit(obj);

        }

        [HttpPost("StudentlistwithID")]
        public Task<List<Emp_StudentReg>> GetStudentWithID(int studentRegID)
        {
            return ServiceManager.GetService<IEmpService>().GetStudentWithID(studentRegID);

        }
    }
}
