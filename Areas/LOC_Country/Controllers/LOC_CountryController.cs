using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Demo_Areas.Areas.LOC_Country.Models;

namespace Demo_Areas.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/[controller]/[action]")]
    public class LOC_CountryController : Controller
    {
        #region INDEX
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Configuration
        private readonly IConfiguration Configuration;

        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region SELECT ALL
        public IActionResult LOC_CountryList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CountryList",dt);
        }
        #endregion

        #region DELETE
        public IActionResult Delete(int CountryID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);
            objCmd.ExecuteNonQuery();

            return RedirectToAction("LOC_CountryList");
        }
        #endregion

        #region ADD EDIT
        public IActionResult Add(int? CountryID)
        {
            if (CountryID != null)
            {
                string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection connection = new SqlConnection(connectionStr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_Country_SelectByPk";
                objCmd.Parameters.AddWithValue("@CountryID", CountryID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();

                dt.Load(objSDR);

                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = (string)dr["CountryName"];
                    modelLOC_Country.CountryCode = (string)dr["CountryCode"];
                }
                return View("LOC_CountryAdd", modelLOC_Country);
            }
            else
            {
                return View("LOC_CountryAdd");
            }
            
        }
        
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if(modelLOC_Country.CountryID == null)
            {
                objCmd.CommandText = "PR_LOC_Country_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_LOC_Country_UpdateByPk";
                objCmd.Parameters.AddWithValue("@CountryID", modelLOC_Country.CountryID);
            }
            
            objCmd.Parameters.AddWithValue("@CountryName", modelLOC_Country.CountryName);
            objCmd.Parameters.AddWithValue("@CountryCode", modelLOC_Country.CountryCode);

            objCmd.ExecuteNonQuery();
            
            connection.Close();

            return RedirectToAction("LOC_CountryList");
        }
        #endregion

        #region SEARCH
        public IActionResult LOC_CountrySearchByName(string? CountryName)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_SelectByCountryName";
            objCmd.Parameters.AddWithValue("@CountryName", CountryName);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CountryList", dt);
        }
        #endregion
    }
}
