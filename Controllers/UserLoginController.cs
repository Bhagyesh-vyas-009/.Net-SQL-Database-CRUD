using Coffee_Shop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Coffee_Shop.Controllers
{
    public class UserLoginController : Controller
    {
        private IConfiguration _configuration;
        public UserLoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login(UserLoginModel userLoginModel)
        {
            try
            {
            if (ModelState.IsValid)
            {
                String str=this._configuration.GetConnectionString("ConnectionString");
                SqlConnection conn=new SqlConnection(str);
                conn.Open();
                SqlCommand cmd=conn.CreateCommand();
                cmd.CommandType=System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_Login";
                cmd.Parameters.AddWithValue("@UserName",userLoginModel.UserName);
                cmd.Parameters.AddWithValue("@Password", userLoginModel.Password);

                SqlDataReader sdr=cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);

                foreach (DataRow dr in dt.Rows) {
                    HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                    HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                    HttpContext.Session.SetString("Email", dr["Email"].ToString());
                }
            }
            return RedirectToAction("ProductList", "Product");
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
        }

        return RedirectToAction("Login");
        }

        public void LogOut()
        {
            HttpContext.Session.Clear();
        }
    }
}
