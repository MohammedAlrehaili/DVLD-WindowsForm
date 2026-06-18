using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTestTypes
    {

        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public short TestTypeFees { get; set; }

        clsTestTypes()
        {
            TestTypeID = -1;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0;
        }

        clsTestTypes(int testTypeID, string testTypeTitle, string testTypeDescription, short testTypeFees)
        {
            this.TestTypeID = testTypeID;
            this.TestTypeTitle = testTypeTitle;
            this.TestTypeDescription = testTypeDescription;
            this.TestTypeFees = testTypeFees;
        }

        public static DataTable GetTestTypes()
        {
            return clsTestTypesDataAccess.GetTestTypes();
        }

        public bool Update()
        {
            return clsTestTypesDataAccess.UpdateTestType(this.TestTypeID,this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
        }

        public static clsTestTypes GetTestTypeByID(int TestTypeID)
        {
            string TestTypeTitle = "";
            string TestTypeDescription = "";
            short TestTypeFees = 0;

            if(clsTestTypesDataAccess.GetTestTypeByID(TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
            {
                return new clsTestTypes(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            return null;
        }
    }
}
