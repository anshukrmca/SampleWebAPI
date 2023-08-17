using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.Core.Entities;

namespace WFM.Core.DTO
{
    public class EmpDTO : Emp
    {
        public string FullName { get { return "WL " + Name ; } }
    }
}
