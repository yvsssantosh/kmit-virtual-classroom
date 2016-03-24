using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace Project
{

    public partial class StudentProfile : System.Web.UI.Page
    {
        string[] notif = new string[100];
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["New"] != null)
            {
                //Response.Write("Welcome");
            }
            else
                Response.Redirect("Login.aspx");
            LogoutB.CssClass = "logout";
           
            if(DropDownList1.SelectedItem.ToString().Equals("All"))
            {
            DropDownList1_SelectedIndexChanged(null,null);
            }

            SqlConnection con = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("select StudentName from StudentData where StudentID='" + Session["New"].ToString() + "'", con);
            string name = cmd.ExecuteScalar().ToString();
           
            LName.Text = "Welcome, " + name;
          
           
            try
            {
                string id = Session["New"].ToString();
                SqlConnection conn = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
                conn.Open();
                int i = 0;
                SqlCommand getnotif = new SqlCommand("select Notifs from Notif where StudentID='" + id + "'", conn);
                SqlDataReader readnotif = getnotif.ExecuteReader();
                if (readnotif.HasRows)
                {
                    while (readnotif.Read())
                    {
                        notif[i] = readnotif.GetString(0);
                        //  Response.Write(notif[i]);
                        i++;

                    }
                }
                for (int j = 0; j < i; j++)
                {
                    Session["index"] = j;
                    notifcreate();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write("ERROR: " + ex.Message);
            }
        }
        protected void create()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableRow row1 = new HtmlTableRow();
            HtmlTableRow row2 = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            HtmlTableCell cell1 = new HtmlTableCell();
            HtmlTableCell cell2 = new HtmlTableCell();
            cell.Controls.Add(this.BuildTLabel());
            cell1.Controls.Add(this.BuildLabel());
            cell2.Controls.Add(this.BuildLink());
            row.Cells.Add(cell);
            row1.Cells.Add(cell1);
            row2.Cells.Add(cell2);
            mytable.Rows.Add(row);
            mytable.Rows.Add(row1);
            mytable.Rows.Add(row2);

        }
        protected virtual Label BuildLabel()
        {
            Label lbl = new Label();
            string tid = Session["tid"].ToString();
            string p = Session["uploadnum"].ToString();
            string path = "F:\\Project\\Project\\Posts\\" + tid + "\\" + p + ".txt";
            StreamReader reads = new StreamReader(path);
            string text = reads.ReadToEnd();
            reads.Dispose();
            lbl.Text = text;
            lbl.ForeColor = System.Drawing.Color.Black;
            
            return lbl;

        }
        protected virtual Label BuildTLabel()
        {
            Label lbl = new Label();
            SqlConnection con = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
            con.Open();
            SqlCommand cmd = new SqlCommand("select TeacherName from TeacherData where TeacherID='" + Session["tid"].ToString() + "'", con);
            string name = cmd.ExecuteScalar().ToString();
            con.Close();
            lbl.Text = Session["uploadnum"].ToString() + ". Prof " + name;
            lbl.ForeColor = System.Drawing.Color.DarkBlue;
            lbl.CssClass = "label";
            return lbl;
        }
        protected virtual LinkButton BuildLink()
        {
            LinkButton link = new LinkButton();
            link.Text = "Download Here" + ".docx";
            link.ForeColor = System.Drawing.Color.DarkBlue;
            link.Click += new EventHandler(this.LinkButton1_Click);
            return link;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

            Response.ContentType = "Application/doc/docx";
            string uploadnum = Session["uploadnum"].ToString();
            string path = Session["path"].ToString();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + uploadnum);
            Response.TransmitFile(Server.MapPath(path));
            Response.End();
        }
        protected void notifcreate()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();

            HtmlTableRow row1 = new HtmlTableRow();
            HtmlTableCell cell1 = new HtmlTableCell();

            cell1.Controls.Add(this.BuildTNLabel());
            cell.Controls.Add(this.BuildNLabel());

            row1.Cells.Add(cell1);
            row.Cells.Add(cell);
       
            notiftable.Rows.Add(row1);
            notiftable.Rows.Add(row);

          
            
           

        }
        protected virtual Label BuildNLabel()
        {
            Label lbl = new Label();
            int index = Convert.ToInt16(Session["index"].ToString());
            lbl.Text = notif[index];
            lbl.ForeColor = System.Drawing.Color.Black;
            return lbl;

        }
        protected virtual Label BuildTNLabel()
        {
            Label lbl = new Label();
            SqlConnection con = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
            con.Open();
            SqlCommand cmd = new SqlCommand("select TeacherName from TeacherData where TeacherID='" + Session["tid"].ToString() + "'", con);
            string name = cmd.ExecuteScalar().ToString();
            con.Close();
            lbl.Text = "Prof. " + name;
            lbl.ForeColor = System.Drawing.Color.DarkBlue;
            lbl.CssClass = "label";
            return lbl;
        }

        protected void LogoutB_Click(object sender, EventArgs e)
        {
            Session["New"] = null;
            Response.Redirect("Login.aspx");

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = DropDownList1.SelectedItem.ToString();
            if (selected.Equals("All"))
            {
                SqlConnection con = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
                con.Open();
                try
                {

                    string id = Session["New"].ToString();
                    SqlCommand values = new SqlCommand("select TeacherID,PC from TeacherData", con);
                    SqlDataReader reads = values.ExecuteReader();
                    int index = 0;
                    string[,] storeval = new string[100, 2];
                    if (reads.HasRows)
                    {

                        while (reads.Read())
                        {

                            storeval[index, 0] = reads.GetString(0);
                            storeval[index, 1] = reads.GetInt32(1).ToString();
                            index++;
                        }

                    }
                    reads.Dispose();
                    SqlCommand cmdprocedure = new SqlCommand("sgets", con);
                    cmdprocedure.CommandType = CommandType.StoredProcedure;
                    cmdprocedure.Parameters.Add(new SqlParameter("@studentid", id));
                    SqlDataReader reader = cmdprocedure.ExecuteReader();
                    int loop = 0;
                    while (reader.Read())
                    {
                        string postscount = null;
                        string tid = reader.GetString(0);
                        while (loop <= index)
                        {
                            if (tid == storeval[loop, 0])
                            {
                                postscount = storeval[loop, 1];
                                Session["postcount"] = postscount;

                            }
                            loop++;
                        }

                        int totalposts = Convert.ToInt32(postscount);
                        int[] post = new int[100];
                        int uploadnum = 1;
                        Session["tid"] = tid;


                        while (uploadnum <= totalposts)
                        {
                            Session["uploadnum"] = uploadnum;
                            string postnum = Convert.ToString(uploadnum);
                            string path = "Posts/" + tid + "/" + postnum + ".docx";
                            string tpath = "Posts\\" + tid + "\\" + postnum + ".txt";
                            Session["tpath"] = tpath;
                            Session["path"] = path;
                            create();
                            uploadnum++;
                        }



                    }







                }
                catch (Exception ex)
                {
                    Response.Write("Error:" + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                SqlConnection conn = new SqlConnection("Data Source=AKANKSHA;Initial Catalog=Project;Integrated Security=True;");
                conn.Open();
                SqlCommand cmd = new SqlCommand("select top " + selected + " TeacherID,PostID from TeacherPost order by Curr_time", conn);
                try
                {
                    SqlDataReader reading = cmd.ExecuteReader();


                    if (reading.HasRows)
                    {
                        while (reading.Read())
                        {
                            Session["tid"] = reading.GetValue(0).ToString();
                            Session["uploadnum"] = reading.GetValue(1).ToString();
                            string path = "Posts/" + reading.GetValue(0).ToString() + "/" + reading.GetValue(1).ToString() + ".docx";
                            string tpath = "Posts\\" + reading.GetValue(0).ToString() + "\\" + reading.GetValue(1).ToString() + ".txt";
                            Session["tpath"] = tpath;
                            Session["path"] = path;
                            create();

                        }
                    }
                    reading.Dispose();
                }
                catch (Exception ex) { 
                }
                conn.Close();

            }
        }
   
           
        


    }
}
