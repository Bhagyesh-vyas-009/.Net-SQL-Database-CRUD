using Coffee_Shop.Helper;
using Coffee_Shop.Models;
using DocumentFormat.OpenXml.EMMA;
using Irony.Parsing;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Coffee_Shop.Controllers
{
    public class CityController : Controller
    {
        #region Configuartion
        public IConfiguration _configuration;

        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region CityList
        public IActionResult CityList()
        {
            string str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_City_SelectAll";

            SqlDataReader sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);
            return View(dt);
        }
        #endregion

        #region CityDelete
        public IActionResult CityDelete(string CityID)
        {
            int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(CityID.ToString()));

            try
            {
                string str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_LOC_City_Delete";
                cmd.Parameters.AddWithValue("@CityID", decryptedCityID);
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("CityList");
        }
        #endregion

        #region StateDropDown
        //public void StateDropDown()
        //{
        //    string connectionString = this._configuration.GetConnectionString("ConnectionString");

        //    SqlConnection connection1 = new SqlConnection(connectionString);
        //    connection1.Open();
        //    SqlCommand command1 = connection1.CreateCommand();
        //    command1.CommandType = System.Data.CommandType.StoredProcedure;
        //    command1.CommandText = "PR_LOC_State_DropDown";
        //    SqlDataReader reader1 = command1.ExecuteReader();
        //    DataTable dataTable1 = new DataTable();
        //    dataTable1.Load(reader1);

        //    List<StateDropDownModel> StateList = new List<StateDropDownModel>();
        //    foreach (DataRow row in dataTable1.Rows)
        //    {
        //        StateDropDownModel state=new StateDropDownModel();
        //        state.StateID = Convert.ToInt32(@row["StateID"]);
        //        state.StateName = @row["StateName"].ToString();
        //        StateList.Add(state);
        //    }

        //    ViewBag.StateList = StateList;
        //}
        #endregion

        #region CountryDropDown
        public void CountryDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_LOC_Country_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);


            List<CountryDropDownModel> CountryList = new List<CountryDropDownModel>();
            foreach (DataRow row in dataTable1.Rows)
            {
                CountryDropDownModel country = new CountryDropDownModel();
                country.CountryID = Convert.ToInt32(row["CountryID"]);
                country.CountryName = row["CountryName"].ToString();
                CountryList.Add(country);
            }

            ViewBag.CountryList = CountryList;
        }
        #endregion

        #region GetStatesByCountry
        [HttpPost]
        public JsonResult GetStatesByCountry(int CountryID)
        {
            List<StateDropDownModel> loc_State = GetStateByCountryID(CountryID); // Fetch states
            return Json(loc_State); // Return JSON response
        }
        #endregion

        #region GetStateByCountryID
        // Helper method to fetch states by country ID
        public List<StateDropDownModel> GetStateByCountryID(int CountryID)
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            List<StateDropDownModel> loc_State = new List<StateDropDownModel>();

            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand objCmd = conn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_LOC_State_SelectComboBoxByCountryID";
                    objCmd.Parameters.AddWithValue("@CountryID", CountryID);

                    using (SqlDataReader objSDR = objCmd.ExecuteReader())
                    {
                        if (objSDR.HasRows)
                        {
                            while (objSDR.Read())
                            {
                                loc_State.Add(new StateDropDownModel
                                {
                                    StateID = Convert.ToInt32(objSDR["StateID"]),
                                    StateName = objSDR["StateName"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return loc_State;
        }
        #endregion

        //#region CityAddEdit
        //public IActionResult CityAddEdit(int CityID)
        //{
        //    CountryDropDown();
        //    string connectionString = this._configuration.GetConnectionString("ConnectionString");
        //    SqlConnection connection1 = new SqlConnection(connectionString);
        //    connection1.Open();
        //    SqlCommand command1 = connection1.CreateCommand();
        //    command1.CommandType = System.Data.CommandType.StoredProcedure;
        //    command1.CommandText = "PR_LOC_City_SelectByPK";

        //    command1.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
        //    SqlDataReader sdr = command1.ExecuteReader();
        //    DataTable dt = new DataTable();
        //    dt.Load(sdr);

        //    CityModel cityModel = new CityModel();
        //    //StateDropDown();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        cityModel.CityID = Convert.ToInt32(dr["CityID"]);
        //        cityModel.CityName = dr["CityName"].ToString();
        //        cityModel.CityCode = dr["CityCode"].ToString();
        //        cityModel.StateID = Convert.ToInt32(dr["StateID"]);
        //        cityModel.CountryID = Convert.ToInt32(dr["CountryID"]);
        //        ViewBag.StateList = GetStateByCountryID(Convert.ToInt32(dr["CountryID"]));
        //    }
        //    GetStatesByCountry(cityModel.CountryID);

        //    return View("CityAddEdit", cityModel);
        //}
        //#endregion

        #region CityAddEdit
        public IActionResult CityAddEdit(int CityID)
        {

                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection1 = new SqlConnection(connectionString);
                connection1.Open();
                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandType = System.Data.CommandType.StoredProcedure;
                command1.CommandText = "PR_LOC_City_SelectByPK";

                command1.Parameters.AddWithValue("@CityID", CityID);
                SqlDataReader sdr = command1.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);

                if (dt.Rows.Count > 0)
                {
                    CityModel cityModel = new CityModel();
                    CountryDropDown();
                    foreach (DataRow dr in dt.Rows)
                    {
                        cityModel.CityID = Convert.ToInt32(dr["CityID"]);
                        cityModel.CityName = dr["CityName"].ToString();
                        cityModel.StateID = Convert.ToInt32(dr["StateID"]);
                        cityModel.CityCode = dr["CityName"].ToString();
                        cityModel.CountryID = Convert.ToInt32(dr["CountryID"]);
                        ViewBag.StateList = GetStateByCountryID(Convert.ToInt32(dr["CountryID"]));
                }
                    GetStatesByCountry(cityModel.CountryID);
                    return View("CityAddEdit", cityModel);
                }
            return View("CityAddEdit");
        }
        #endregion

        #region CitySave
        [HttpPost]
        public IActionResult CitySave(CityModel cityModel)
        {

            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (cityModel.CityID == 0 || cityModel.CityID == null)
                {
                    command.CommandText = "PR_LOC_City_Insert";
                }
                else
                {
                    command.CommandText = "PR_LOC_City_Update";
                    command.Parameters.AddWithValue("@CityID",cityModel.CityID);
                }
                command.Parameters.AddWithValue("@CityName", cityModel.CityName);
                command.Parameters.AddWithValue("@CityCode", cityModel.CityCode);
                command.Parameters.AddWithValue("@StateID", cityModel.StateID);
                command.Parameters.AddWithValue("@CountryID", cityModel.CountryID);

                command.ExecuteNonQuery();
                return RedirectToAction("CityList");
            }
            //StateDropDown();
            CountryDropDown();
            return View("CityAddEdit",cityModel);
        }
        #endregion
    }
}
