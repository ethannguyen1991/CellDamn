﻿using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
public partial class Login : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CPDBCS"].ConnectionString);
    SqlCommand cmd = new SqlCommand();
    SqlDataAdapter adp;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "STUDENT_LOGIN";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = txtUser.Text;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = txtPass.Text;
            cmd.Parameters.Add("@StudentId", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@IsAllowEdit", SqlDbType.Bit);
            cmd.Parameters["@StudentId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@IsAllowEdit"].Direction = ParameterDirection.Output;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            string studentID=string.Empty;
            Boolean isAllowEdit=false;
            if (cmd.Parameters["@StudentId"].Value!=null)
             studentID = cmd.Parameters["@StudentId"].Value.ToString();
            if(!Convert.IsDBNull(cmd.Parameters["@IsAllowEdit"].Value))
            isAllowEdit =Convert.ToBoolean(cmd.Parameters["@IsAllowEdit"].Value);
            
            if (!string.IsNullOrEmpty(studentID))
            {
                Session["UserID"] = studentID;
                Session["IsStudent"] = 1;
                Session["IsAllowEdit"] = isAllowEdit;
                Session["UserName"] = txtUser.Text;
                Response.Redirect("Home.aspx", false);
                ApplicationInstance.CompleteRequest();
            }
            else if (true)
              {
                cmd.Connection = con;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                cmd.CommandText = "AdminLogin";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = txtUser.Text;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = txtPass.Text;
                cmd.Parameters.Add("@AdminID", SqlDbType.VarChar, 50);
                cmd.Parameters["@AdminID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string adminID = cmd.Parameters["@AdminID"].Value.ToString();
                if (!string.IsNullOrEmpty(adminID))
                {
                    Session["UserID"] = adminID;
                    Session["IsStudent"] = 0;
                    Session["UserName"] = txtUser.Text;
                    Response.Redirect("Home.aspx", false);
                    ApplicationInstance.CompleteRequest();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('The Username or Password is incorrect.')", true);
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('The Username or Password is incorrect.')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "alert", "alert('" + ex.Message + "')", true);
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}