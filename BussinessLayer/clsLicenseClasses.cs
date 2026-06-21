using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLicenseClasses
    {


        public static DataTable GetLicenseClasses()
        {
            return clsLicenseClassesDataAccess.GetLicenseClasses();
        }
    }
}
