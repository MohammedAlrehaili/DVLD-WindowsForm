using System;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BussinessLayer
{
    public class clsCountry
    {

        public int CountryID { get; set; }
        public string CountryName { get; set; }

        clsCountry()
        {
            CountryID = -1;
            CountryName = "";
        }

        clsCountry(int countryID, string countryName)   
        {
            CountryID = countryID;
            CountryName = countryName;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();
        }

        public static string GetCountryName(int countryID)
        {
            return clsCountryDataAccess.GetCountryNameByID(countryID);
        }
    }
}
