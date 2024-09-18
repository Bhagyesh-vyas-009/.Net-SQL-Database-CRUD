using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data;
using System.Data.SqlClient;
using Coffee_Shop.BAL;
namespace Coffee_Shop.Controllers
{
    public class UserController : Controller
    {
        #region Configuration
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        [CheckAccess]
        #region UserList
        public IActionResult UserList()
        {
            String str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn=new SqlConnection(str);

            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_User_SelectAll";

            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            return View(dt);
        }
        #endregion

        #region UserDelete
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "pr_User_Delete";
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                cmd.ExecuteNonQuery();
                TempData["Success"] = "Deleted successfully.";
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "You can't delete this user because of foreign key constraint";
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("UserList");
        }
        #endregion

        [CheckAccess]
        #region UserAddEdit
        public IActionResult UserAddEdit(int UserID)
        {
            String str = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_User_SelectByPK";
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            SqlDataReader sdr = cmd.ExecuteReader();
            DataTable dt=new DataTable();
            dt.Load(sdr);

            UserModel userModel = new UserModel();
            foreach(DataRow dr in dt.Rows)
            {
                userModel.UserID = Convert.ToInt32(@dr["UserID"]);
                userModel.UserName = @dr["UserName"].ToString();
                userModel.Email = @dr["Email"].ToString();
                userModel.Password = @dr["Password"].ToString();
                userModel.MobileNo = @dr["MobileNo"].ToString();
                userModel.Address = @dr["Address"].ToString();
                userModel.isActive = Convert.ToBoolean(dr["isActive"]);
            }
            conn.Close();
            return View("UserAddEdit",userModel);
        }
        #endregion

        [CheckAccess]
        #region UserSave
        [HttpPost]
        public IActionResult UserSave(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                String str = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = userModel.isActive;

                if (userModel.UserID == null || userModel.UserID == 0)
                {
                    cmd.CommandText = "PR_User_Insert";
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "New Record Inserted Successfully";
                    return RedirectToAction("UserAddEdit");
                }
                else
                {
                    cmd.CommandText = "PR_User_Update";
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
                    cmd.ExecuteNonQuery();
                    TempData["Message"] = "Record Updated Successfully";
                    return RedirectToAction("UserList");
                }
            }
            else
            {
                return RedirectToAction("AddEditUser");
            }
        }
        #endregion

        #region Login
        //[HttpPost]
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String str = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection conn = new SqlConnection(str);
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "PR_User_Login";
                    cmd.Parameters.AddWithValue("@UserName", userLoginModel.UserName);
                    cmd.Parameters.AddWithValue("@Password", userLoginModel.Password);

                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                            HttpContext.Session.SetString("Email", dr["Email"].ToString());
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    return View("Login",userLoginModel);
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        #endregion

        #region LogOut
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion

    }
}
