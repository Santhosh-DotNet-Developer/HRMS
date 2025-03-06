using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models
{
    interface IRepository<T>
    {
        List<T> GetList();
        T GetDetails(string p);
        string Save(T entity);
        string Delete(string p);
        bool CheckDeptCodeExists(string p);
        bool CheckDeptNameExists(string p1, string p2, string p3);
    }
}
