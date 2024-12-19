using Coffee_Shop.Helper;
using Coffee_Shop.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
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
            this._configuration = configuration;
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

            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_LOC_State_SelectComboBoxByCountryID";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    loc_State.Add(new StateDropDownModel
                    {
                        StateID = Convert.ToInt32(sdr["StateID"]),
                        StateName = sdr["StateName"].ToString()
                    });
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

        public IActionResult CityAddEdit(string? CID)
        {
            int? decryptedCityID = null;
            if (!string.IsNullOrEmpty(CID))
            {
                string decryptedCityIDString = UrlEncryptor.Decrypt(CID); // Decrypt the encrypted CityID
                decryptedCityID = int.Parse(decryptedCityIDString);
            }
            //DropDown dropDown = new DropDown(_configuration);

            //ViewBag.countryList = dropDown.CountryDropdown();
            CountryDropDown();

            //ViewBag.Id = CityID;
            CityModel cityModel = new CityModel();

            if (decryptedCityID != null)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command2 = connection.CreateCommand();
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.CommandText = "PR_LOC_City_SelectByPK";
                command2.Parameters.Add("@CityID", SqlDbType.Int).Value = decryptedCityID;
                SqlDataReader reader2 = command2.ExecuteReader();
                DataTable dataTable2 = new DataTable();
                dataTable2.Load(reader2);
                connection.Close();
                foreach (DataRow dataRow in dataTable2.Rows)
                {
                    cityModel.CityID = Convert.ToInt32(dataRow["CityID"]);
                    cityModel.CityName = Convert.ToString(dataRow["CityName"]);
                    cityModel.StateID = Convert.ToInt32(dataRow["StateID"]);
                    cityModel.CountryID = Convert.ToInt32(dataRow["CountryID"]);
                    cityModel.CityCode = Convert.ToString(dataRow["CityCode"]);
                }
            }
            ViewBag.stateList = GetStateByCountryID(cityModel.CountryID);
            return View(cityModel);
        }

        public IActionResult CitySave(CityModel cityModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (cityModel.CityID == null || cityModel.CityID == 0)
                {
                    command.CommandText = "PR_LOC_City_Insert";
                    command.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DBNull.Value;
                }
                else
                {
                    command.CommandText = "PR_LOC_City_Update";
                    command.Parameters.Add("@CityID", SqlDbType.Int).Value = cityModel.CityID;
                }
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = cityModel.CityName;
                command.Parameters.Add("@StateID", SqlDbType.Int).Value = cityModel.StateID;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = cityModel.CountryID;
                command.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = cityModel.CityCode;
                command.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = DBNull.Value;
                command.ExecuteNonQuery();
                if (cityModel.CityID != 0) { return RedirectToAction("CityList"); }
                else
                {
                    ModelState.Clear();
                    return RedirectToAction("CityAddEdit");
                }

            }
            else
            {
                return RedirectToAction("CityAddEdit");
            }
        }
    }
}
