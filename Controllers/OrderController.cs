using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data;
using System.Data.SqlClient;
using Coffee_Shop.BAL;

namespace Coffee_Shop.Controllers
{
    [CheckAccess]
    public class OrderController : Controller
    {
        #region Configuration
        public IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        #endregion

        #region OrderList
        public IActionResult OrderList()
        {
            String str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn=new SqlConnection(str);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Order_SelectAll";

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            return View(dt);
        }
        #endregion

        #region OrderDelete
        public IActionResult OrderDelete(int OrderID)
        {
            
            try
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "pr_Order_Delete";
                cmd.Parameters.AddWithValue("@OrderID", OrderID);
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "You can't delete this order because of foreign key constraint";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("OrderList");
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

        #region CustomerDropDown
        public void CustomerDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Customer_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<CustomerDropDownModel> customerList = new List<CustomerDropDownModel>();
            foreach (DataRow dataRow in dataTable1.Rows)
            {
                CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                customerDropDownModel.CustomerID = Convert.ToInt32(dataRow["CustomerID"]);
                customerDropDownModel.CustomerName = dataRow["CustomerName"].ToString();
                customerList.Add(customerDropDownModel);
            }
            ViewBag.CustomerList = customerList;
        }
        #endregion

        #region OrderAddEdit
        public IActionResult OrderAddEdit(int OrderID)
        {
            #region orderByID
            String connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectByPK";
            command.Parameters.AddWithValue("@OrderID", OrderID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            OrderModel orderModel = new OrderModel();
            UserDropDown();
            CustomerDropDown();
            foreach (DataRow dataRow in table.Rows)
            {
                orderModel.OrderID = Convert.ToInt32(@dataRow["orderID"]);
                orderModel.OrderDate = Convert.ToDateTime(@dataRow["OrderDate"]);
                orderModel.OrderNumber = @dataRow["OrderNumber"].ToString();
                orderModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                orderModel.PaymentMode = @dataRow["PaymentMode"].ToString();
                orderModel.TotalAmount = Convert.ToDecimal(@dataRow["TotalAmount"]);
                orderModel.ShippingAddress = @dataRow["ShippingAddress"].ToString();
                orderModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View("OrderAddEdit", orderModel);

        }
        #endregion

        #region OrderSave
        [HttpPost]
        public IActionResult OrderSave(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection1 = new SqlConnection(connectionString);
                connection1.Open();
                SqlCommand cmd = connection1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if (orderModel.OrderID == null || orderModel.OrderID == 0)
                {
                    cmd.CommandText = "PR_Order_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_Order_Update";
                    cmd.Parameters.AddWithValue("@OrderID",orderModel.OrderID);
                }
                cmd.Parameters.AddWithValue("@OrderDate", orderModel.OrderDate);
                cmd.Parameters.AddWithValue("@OrderNumber", orderModel.OrderNumber);
                cmd.Parameters.AddWithValue("@CustomerID",orderModel.CustomerID);
                cmd.Parameters.AddWithValue("@PaymentMode",orderModel.PaymentMode);
                cmd.Parameters.AddWithValue("@TotalAmount", orderModel.TotalAmount);
                cmd.Parameters.AddWithValue("@ShippingAddress",orderModel.ShippingAddress);
                cmd.Parameters.AddWithValue("@UserID",orderModel.UserID);

                cmd.ExecuteNonQuery();
                return RedirectToAction("OrderList");
            }
            return View("OrderAddEdit", orderModel);
        }
        #endregion
    }
}
