using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data;
using System.Data.SqlClient;
using Coffee_Shop.BAL;

namespace Coffee_Shop.Controllers
{
    [CheckAccess]
    public class BillsController : Controller
    {
        #region Configuration
        public IConfiguration _configuration;

        public BillsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region BillsList
        public IActionResult BillsList()
        {
            String str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Bills_SelectAll";

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            return View(dt);
        }
        #endregion

        #region BillDelete
        public IActionResult BillDelete(int BillID)
        {
            try
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "pr_Bill_Delete";
                cmd.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("BillsList");
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

        #region OrderDropDown
        public void OrderDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            connection1.Close();

            List<OrderDropDownModel> orders = new List<OrderDropDownModel>();

            foreach (DataRow dataRow in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(dataRow["OrderID"]);
                orderDropDownModel.OrderNumber = dataRow["OrderNumber"].ToString();

                orders.Add(orderDropDownModel);
            }

            ViewBag.OrderList = orders;
        }
        #endregion

        #region BillAddEdit
        public IActionResult BillAddEdit(int BillID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Bills_SelectByPK";
            command1.Parameters.AddWithValue("@BillID",BillID);
            SqlDataReader sdr = command1.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);


            BillsModel billsModel = new BillsModel();
            UserDropDown();
            OrderDropDown();
            foreach (DataRow dr in dt.Rows)
            {
                billsModel.BillID = Convert.ToInt32(dr["BillID"]);
                billsModel.BillNumber =@dr["BillNumber"].ToString();
                billsModel.BillDate = Convert.ToDateTime(dr["BillDate"]);
                billsModel.OrderID = Convert.ToInt32(dr["OrderID"]);
                billsModel.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                billsModel.Discount = Convert.ToDecimal(dr["Discount"]);
                billsModel.NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                billsModel.UserID = Convert.ToInt32(dr["UserID"]);
            }
            connection1.Close();
            return View("BillAddEdit", billsModel);
        }
        #endregion

        #region BillSave
        [HttpPost]
        public IActionResult BillSave(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection1 = new SqlConnection(connectionString);
                connection1.Open();
                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandType = System.Data.CommandType.StoredProcedure;

                if (billsModel.BillID == null || billsModel.BillID == 0)
                {
                    command1.CommandText = "pr_Bill_INSERT";
                }
                else
                {
                    command1.CommandText = "pr_Bill_UPDATE";
                    command1.Parameters.Add("@BillID", SqlDbType.Int).Value = billsModel.BillID;
                }
                command1.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billsModel.BillNumber;
                command1.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                command1.Parameters.Add("@OrderID", SqlDbType.Int).Value = billsModel.OrderID;
                command1.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                command1.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                command1.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;
                command1.Parameters.Add("@UserID", SqlDbType.Int).Value = billsModel.UserID;

                command1.ExecuteNonQuery();
                return RedirectToAction("BillsList");
            }

            return View("BillAddEdit", billsModel);
        }
        #endregion
    }
}
