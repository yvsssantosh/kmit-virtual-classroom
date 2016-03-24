﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Project
{
    public partial class TeacherSubscribe : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {

          
            string id = Session["New"].ToString();
            SqlConnection conn = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
            conn.Open();
            SqlCommand check = new SqlCommand("select StudentID,SubscribedTID from StudentSubscribed", conn);
            SqlDataReader reads = check.ExecuteReader();
            int i = 0;
            if (reads.HasRows)
            {
                while (i < GridView1.Rows.Count && reads.Read())
                {
                    if (reads.GetString(0) == id && reads.GetString(1) == GridView1.Rows[i].Cells[0].Text)
                        GridView1.Rows[i].Cells[3].Enabled = false;
                    else
                        GridView1.Rows[i].Cells[3].Enabled = true;
                    i++;

                }
            }

            reads.Dispose();
            conn.Close();
        }

        protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
        {


        }
        protected void subscribe_clicked(object sender, EventArgs e)
        {
            try
            {
                GridViewRow grdrow = (GridViewRow)((Button)sender).NamingContainer;
                string tid = grdrow.Cells[0].Text;
        
                string id = Session["New"].ToString();
                SqlConnection conn = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
                conn.Open();
                SqlCommand insert = new SqlCommand("insert into StudentSubscribed(StudentID,SubscribedTID) values('" + id + "','" + tid + "')", conn);
                insert.ExecuteNonQuery();
                conn.Close();
                grdrow.Cells[3].Enabled = false;
            }
            catch (Exception ex)
            {
                Response.Write("ERROR:" + ex.Message);
            }
        }
        protected void LogoutB_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("Login.aspx");

        }


    }
}