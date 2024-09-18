using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data;
using System.Data.SqlClient;
using Coffee_Shop.BAL;

namespace Coffee_Shop.Controllers
{
    [CheckAccess]
    public class OrderDetailController : Controller
    {
        #region Configuration
        public IConfiguration _configuration;

        public OrderDetailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion


        #region OrderDetailList
        public IActionResult OrderDetailList()
        {
            String str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_OrderDetail_SelectAll";

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            return View(dt);
        }
        #endregion

        #region OrderDetailDelete
        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
            try
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "pr_OrderDetail_Delete";
                cmd.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("OrderDetailList");
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

        #region ProductDropDown
        public void ProductDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Product_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<ProductDropDownModel> productList = new List<ProductDropDownModel>();
            foreach (DataRow dataRow in dataTable1.Rows)
            {
                ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                productDropDownModel.ProductID = Convert.ToInt32(dataRow["ProductID"]);
                productDropDownModel.ProductName = dataRow["ProductName"].ToString();
                productList.Add(productDropDownModel);
            }
            ViewBag.ProductList = productList;
        }
        #endregion

        #region OrderDetailAddEdit
        public IActionResult OrderDetailAddEdit(int OrderDetailID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_OrderDetail_SelectByPK";
            command1.Parameters.Add("@OrderDetailID",SqlDbType.Int).Value = OrderDetailID;
            SqlDataReader sdr = command1.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);

            UserDropDown();
            ProductDropDown();
            OrderDropDown();
            OrderDetailModel orderDetailModel = new OrderDetailModel();
            foreach(DataRow dr in dt.Rows)
            {
                orderDetailModel.OrderDetailID = Convert.ToInt32(dr["OrderDetailID"]);
                orderDetailModel.OrderID = Convert.ToInt32(dr["OrderID"]);
                orderDetailModel.ProductID = Convert.ToInt32(dr["ProductID"]);
                orderDetailModel.Quantity = Convert.ToInt32(dr["Quantity"]);
                orderDetailModel.Amount = Convert.ToDecimal(dr["Amount"]);
                orderDetailModel.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                orderDetailModel.UserID = Convert.ToInt32(dr["UserID"]);
            }
            
            return  View("OrderDetailAddEdit",orderDetailModel);
        }
        #endregion

        #region OrderDetailSave
        [HttpPost]
        public IActionResult OrderDetailSave(OrderDetailModel orderDetailModel)
        {
                if (orderDetailModel.UserID <= 0)
                {
                    ModelState.AddModelError("UserID", "A valid User is required.");
                }
                if (orderDetailModel.ProductID <= 0)
                {
                    ModelState.AddModelError("ProductID", "A valid Product is required.");
                }

                if (ModelState.IsValid)
                {
                    UserDropDown();
                    ProductDropDown();
                    OrderDropDown();
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    if (orderDetailModel.OrderDetailID == null || orderDetailModel.OrderDetailID == 0)
                    {
                        command.CommandText = "pr_OrderDetail_Insert";
                    }
                    else
                    {
                        command.CommandText = "pr_OrderDetail_Update";
                        command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderDetailModel.OrderDetailID;
                    }
                    command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderDetailModel.OrderID;
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderDetailModel.ProductID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderDetailModel.Quantity;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = orderDetailModel.Amount;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderDetailModel.TotalAmount;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderDetailModel.UserID;

                    command.ExecuteNonQuery();
                    return RedirectToAction("OrderDetailList");
                }

                return View("OrderDetailAddEdit", orderDetailModel);
        }
        #endregion
    }
}
