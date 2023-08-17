using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.Core.DTO;

namespace WFM.Core.Entities
{
    public class Emp
    {
        public string Name { get; set; }
        public string StuID { get; set; }
        public string Email { get; set; }
        public string Father_name { get; set; }
        public string Mother_name { get; set; }
        public string phone { get; set; }
        public string Departmentname { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string state { get; set; }
        public string counrty { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }

    public class EmpgetDetail
    {
        public string AutoCode { get; set; }
        public string Name { get; set; }
        public string StuID { get; set; }
        public string Email { get; set; }
        public string Father_name { get; set; }
        public string Mother_name { get; set; }
        public string phone { get; set; }
        public string Departmentname { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string state { get; set; }
        public string counrty { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Emp_StudentReg
    {
        public int StudentRegID { get; set; }
        public string Name { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

 
}
