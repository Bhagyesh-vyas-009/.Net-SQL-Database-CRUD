using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Coffee_Shop.BAL;

namespace Coffee_Shop.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        #region Configuration
        public IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region ProductList
        public IActionResult ProductList()
        {
            String str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn=new SqlConnection(str);
            conn.Open();
            SqlCommand cmd=conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Product_SelectAll";

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            return View(dt);
        }
        #endregion

        #region ProductDelete
        public IActionResult ProductDelete(int ProductID)
        {
            try
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "pr_Product_Delete";
                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]= "You can't delete this product because of foreign key constraint";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("ProductList");
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

        #region PeoductAddEdit
        public IActionResult ProductAddEdit(int ProductID)
        {
            #region ProductByID
                String connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_SelectByPK";
                command.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                ProductModel productModel = new ProductModel();
                UserDropDown(); 
                foreach (DataRow dataRow in table.Rows)
                {
                    productModel.ProductID = Convert.ToInt32(@dataRow["ProductID"]);
                    productModel.ProductName = @dataRow["ProductName"].ToString();
                    productModel.ProductCode = @dataRow["ProductCode"].ToString();
                    productModel.ProductPrice = Convert.ToDouble(@dataRow["ProductPrice"]);
                    productModel.Description = @dataRow["Description"].ToString();
                    productModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
                }

                #endregion

                return View("ProductAddEdit", productModel);
        }
        #endregion

        #region ProductSave

        [HttpPost]
        public IActionResult ProductSave(ProductModel productModel)
        {
            if (productModel.UserID <= 0)
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
                if (productModel.ProductID == null || productModel.ProductID==0)
                {
                    command.CommandText = "pr_Product_Insert";
                }
                else
                {
                    command.CommandText = "pr_Product_Update";
                    command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
                }
                command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;

                command.ExecuteNonQuery();
                //ViewBag.Message = productModel.ProductID == 0 ? "Product added successfully!" : "Product updated successfully!";
                //if(productModel.ProductID == 0)
                //{
                //    ModelState.Clear();
                //}
                //else
                //{
                    return RedirectToAction("ProductList");
                //}
            }

            return View("ProductAddEdit", productModel);
        }
        #endregion
    }
}
