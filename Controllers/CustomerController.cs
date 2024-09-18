using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data.SqlClient;
using System.Data;
using Coffee_Shop.BAL;

namespace Coffee_Shop.Controllers
{
    [CheckAccess]
    public class CustomerController : Controller
    {
        #region Configuration
        public IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region CustomerList
        public IActionResult CustomerList()
        {
            String str = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection conn = new SqlConnection(str);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Customer_SelectAll";

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            return View(dt);
        }
        #endregion

        #region CustomerDelete
        public IActionResult CustomerDelete(int CustomerID)
        {
            try
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "pr_Customer_Delete";
                cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("CustomerList");
        }
        #endregion

        #region UserDropDown
        public void UserDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            connection1.Close();

            List<UserDropDownModel> users = new List<UserDropDownModel>();

            foreach (DataRow dataRow in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(dataRow["UserID"]);
                userDropDownModel.UserName = dataRow["UserName"].ToString();
                users.Add(userDropDownModel);
            }

            ViewBag.UserList = users;
        }
        #endregion

        #region CustomerAddEdit
        public IActionResult CustomerAddEdit(int CustomerID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Customer_SelectByPK";
            
            command1.Parameters.Add("@CustomerID",SqlDbType.Int).Value=CustomerID;
            SqlDataReader sdr=command1.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(sdr);

            CustomerModel customerModel = new CustomerModel();
            UserDropDown();
            foreach(DataRow dr in dt.Rows)
            {
                customerModel.CustomerID = Convert.ToInt32(@dr["CustomerID"]);
                customerModel.CustomerName = @dr["CustomerName"].ToString();
                customerModel.HomeAddress = @dr["HomeAddress"].ToString();
                customerModel.Email = @dr["Email"].ToString();
                customerModel.MobileNo = @dr["MobileNo"].ToString();
                customerModel.GSTNO = @dr["GSTNO"].ToString();
                customerModel.CityName = @dr["CityName"].ToString();
                customerModel.PinCode = @dr["PinCode"].ToString();
                customerModel.NetAmount = Convert.ToDecimal(@dr["NetAmount"]);
                customerModel.UserID = Convert.ToInt32(@dr["UserID"]);
            }
            connection1.Close();

            return View("CustomerAddEdit", customerModel);
        }
        #endregion

        #region CustomerSave
        [HttpPost]
        public IActionResult CustomerSave(CustomerModel  customerModel)
        {
            if (customerModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {

                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (customerModel.CustomerID == null || customerModel.CustomerID == 0)
                {
                    command.CommandText = "pr_Customer_Insert";
                }
                else
                {
                    command.CommandText = "pr_Customer_Update";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerModel.CustomerID;
                }
                command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerModel.CustomerName;
                command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = customerModel.HomeAddress;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = customerModel.Email;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = customerModel.MobileNo ;
                command.Parameters.Add("@GSTNO", SqlDbType.VarChar).Value =customerModel.GSTNO ;
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value =customerModel.CityName;
                command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value =customerModel.PinCode;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customerModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = customerModel.UserID;
                

                command.ExecuteNonQuery();
                return RedirectToAction("CustomerList");
            }
            UserDropDown();

            return View("CustomerAddEdit", customerModel);
        }
        #endregion
    }
}
