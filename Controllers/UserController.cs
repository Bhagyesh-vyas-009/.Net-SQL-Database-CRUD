﻿using Microsoft.AspNetCore.Mvc;
using Coffee_Shop.Models;
using System.Data;
using System.Data.SqlClient;

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
                if (userModel.UserID == null || userModel.UserID == 0)
                {
                    cmd.CommandText = "PR_User_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_User_Update";
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
                }
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                cmd.Parameters.Add("@isActive", SqlDbType.Bit).Value = userModel.isActive;

                cmd.ExecuteNonQuery();
                
                return RedirectToAction("UserList");
            }
            return View("UserAddEdit", userModel);
        }
        #endregion
    }
}
