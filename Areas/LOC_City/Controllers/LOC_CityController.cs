using Demo_Areas.Areas.LOC_City.Models;
using Demo_Areas.Areas.LOC_Country.Models;
using Demo_Areas.Areas.LOC_State.Models;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Demo_Areas.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/[controller]/[action]")]
    public class LOC_CityController : Controller
    {
        #region INDEX
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region CONFIGURATION
        private readonly IConfiguration Configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        #endregion

        #region SELECT ALL
        public IActionResult LOC_CityList()
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_LOC_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            #region State DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_LOC_State_ComboBox";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_StateDropDownModel stateModel = new LOC_StateDropDownModel();
                stateModel.StateID = Convert.ToInt32(dr["StateID"]);
                stateModel.StateName = dr["StateName"].ToString();
                list2.Add(stateModel);
            }
            ViewBag.StateList = list2;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_SelectAll";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            connection.Close();

            return View("LOC_CityList", dt);
        }
        #endregion

        #region DELETE
        public IActionResult Delete(int CityID)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_DeleteByPK";
            objCmd.Parameters.AddWithValue("@CityID", CityID);
            objCmd.ExecuteNonQuery();
            return RedirectToAction("LOC_CityList");
        }
        #endregion

        #region ADD EDIT
        public IActionResult Add(int? CityID)   
        {
            #region ComboBox

            string connectionstr = this.Configuration.GetConnectionString("myConnectionString");

            DataTable dt1 = new DataTable();
            SqlConnection conn1 = new SqlConnection(connectionstr);
            conn1.Open();
            SqlCommand objCmd1 = conn1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_LOC_Country_ComboBox";
            SqlDataReader objSDR1 = objCmd1.ExecuteReader();
            dt1.Load(objSDR1);

            List<LOC_CountryDropDownModel> list = new List<LOC_CountryDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryDropDownModel vlst = new LOC_CountryDropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = dr["CountryName"].ToString();
                list.Add(vlst);
            }

            ViewBag.CountryList = list;

            #endregion

            List<LOC_StateDropDownModel> list1 = new List<LOC_StateDropDownModel>();
            ViewBag.StateList = list1;

            if (CityID != null)
            {
                SqlConnection connection = new SqlConnection(connectionstr);
                connection.Open();
                SqlCommand objCmd = connection.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_LOC_City_SelectByPk";
                objCmd.Parameters.AddWithValue("@CityID", CityID);
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                dt.Load(objSDR);

                LOC_CityModel modelLOC_City = new LOC_CityModel();

                foreach (DataRow dr in dt.Rows)
                {
                    modelLOC_City.CityName = (string)dr["CityName"];
                    modelLOC_City.StateName = (string)dr["StateName"];
                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CityCode = (string)dr["CityCode"];
                }

                DataTable dt2 = new DataTable();
                SqlConnection conn2 = new SqlConnection(connectionstr);
                conn2.Open();
                SqlCommand objCmd2 = conn1.CreateCommand();
                objCmd2.CommandType = CommandType.StoredProcedure;
                objCmd2.CommandText = "PR_LOC_State_ComboBoxbyCountryId";
                objCmd2.Parameters.AddWithValue("@CountryID", modelLOC_City.CountryID);
                SqlDataReader objSDR2 = objCmd2.ExecuteReader();
                dt2.Load(objSDR2);

                List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();

                foreach (DataRow dr in dt2.Rows)
                {
                    LOC_StateDropDownModel vlst = new LOC_StateDropDownModel();
                    vlst.StateID = Convert.ToInt32(dr["StateID"]);
                    vlst.StateName = dr["StateName"].ToString();
                    list2.Add(vlst);
                }
                ViewBag.StateList = list2;

                return View("LOC_CityAdd", modelLOC_City);

            }
            else
            {
                return View("LOC_CityAdd");
            }
        }
        #endregion

        #region SAVE
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;

            if (modelLOC_City.CityID == null)
            {
                objCmd.CommandText = "PR_LOC_City_Insert";
            }
            else
            {
                objCmd.CommandText = "PR_LOC_City_UpdateByPk";
                objCmd.Parameters.AddWithValue("@CityID", modelLOC_City.CityID);
            }

            objCmd.Parameters.AddWithValue("@CityName", modelLOC_City.CityName);
            objCmd.Parameters.AddWithValue("@StateID", modelLOC_City.StateID);
            objCmd.Parameters.AddWithValue("@CountryID", modelLOC_City.CountryID);
            objCmd.Parameters.AddWithValue("@CityCode", modelLOC_City.CityCode);

            objCmd.ExecuteNonQuery();

            connection.Close();

            return RedirectToAction("LOC_CityList");
        }
        #endregion

        #region SEARCH BY NAME
        public IActionResult LOC_CitySearchByName(string? CityName)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_SelectByCityName";
            objCmd.Parameters.AddWithValue("@CityName", CityName);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);
            return View("LOC_CityList", dt);
        }
        #endregion

        #region Cascade DropDown of State
        public IActionResult StateDropDownByCountryID(int CountryID)
        {
            string myconnStr1 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection1 = new SqlConnection(myconnStr1);
            DataTable dt1 = new DataTable();
            connection1.Open();
            SqlCommand cmd1 = connection1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_LOC_State_ComboBoxbyCountryId";
            cmd1.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            dt1.Load(reader1);

            List<LOC_StateDropDownModel> list = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_StateDropDownModel lstList = new LOC_StateDropDownModel();
                lstList.StateID = Convert.ToInt32(dr["StateID"]);
                lstList.StateName = dr["StateName"].ToString();
                list.Add(lstList);
            }
            var vModel = list;
            return Json(vModel);

        }



        #endregion

        #region FILTER
        public IActionResult LOC_CityFilter(LOC_CityFilterModel FilterModel)
        {
            string connectionStr = this.Configuration.GetConnectionString("myConnectionString");

            #region Country DropDown

            SqlConnection connection1 = new SqlConnection(connectionStr);
            connection1.Open();
            SqlCommand objCmd1 = connection1.CreateCommand();
            objCmd1.CommandType = CommandType.StoredProcedure;
            objCmd1.CommandText = "PR_LOC_Country_ComboBox";
            SqlDataReader reader1 = objCmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(reader1);
            connection1.Close();

            List<LOC_CountryModel> list = new List<LOC_CountryModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                LOC_CountryModel countryModel = new LOC_CountryModel();
                countryModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                countryModel.CountryName = dr["CountryName"].ToString();
                list.Add(countryModel);
            }
            ViewBag.CountryList = list;

            #endregion

            #region State DropDown

            SqlConnection connection2 = new SqlConnection(connectionStr);
            connection2.Open();
            SqlCommand objCmd2 = connection2.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_LOC_State_ComboBox";
            SqlDataReader reader2 = objCmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(reader2);
            connection2.Close();

            List<LOC_StateDropDownModel> list2 = new List<LOC_StateDropDownModel>();

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_StateDropDownModel stateModel = new LOC_StateDropDownModel();
                stateModel.StateID = Convert.ToInt32(dr["StateID"]);
                stateModel.StateName = dr["stateName"].ToString();
                list2.Add(stateModel);
            }
            ViewBag.StateList = list2;

            #endregion

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand objCmd = connection.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_filter";
            objCmd.Parameters.AddWithValue("@CountryID", FilterModel.CountryID);
            objCmd.Parameters.AddWithValue("@StateID", FilterModel.StateID);
            objCmd.Parameters.AddWithValue("@CityName", FilterModel.CityName);
            objCmd.Parameters.AddWithValue("@CityCode", FilterModel.CityCode);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            ModelState.Clear();
            return View("LOC_CityList", dt);
        }
        #endregion
    }
}
