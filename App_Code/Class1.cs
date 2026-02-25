using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using com.flajaxian;

namespace DocProc
{
    public class Godd: FileUploaderAdapter
    {
        public string groupid;
        public override void ProcessFile(HttpPostedFile file)
        {
            SqlConnection sqlConn = new SqlConnection("Server=127.0.0.1,667;Initial Catalog=bluewaterintranet;user id=bluewater;Password=bwintranet;");
            SqlCommand sqlComm = new SqlCommand("insert into documents(name,filename,groupid,mime) values(@name,@filename,@groupid,@mime)", sqlConn);
            sqlComm.Parameters.AddWithValue("@name", file.FileName.Split('.')[0]);
            sqlComm.Parameters.AddWithValue("@groupid", groupid);
            sqlComm.Parameters.AddWithValue("@filename", file.FileName);
            sqlComm.Parameters.AddWithValue("@mime", System.IO.Path.GetExtension(file.FileName));
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();

            
        }
    }

    public class Godd2 : FileUploaderAdapter
    {
        public string groupid;
        public override void ProcessFile(HttpPostedFile file)
        {
            SqlConnection sqlConn = new SqlConnection("Server=127.0.0.1,667;Initial Catalog=bluewaterintranet;user id=bluewater;Password=bwintranet;");
            SqlCommand sqlComm = new SqlCommand("insert into photos(name,filename,groupid,mime) values(@name,@filename,@groupid,@mime)", sqlConn);
            sqlComm.Parameters.AddWithValue("@name", file.FileName.Split('.')[0]);
            sqlComm.Parameters.AddWithValue("@groupid", groupid);
            sqlComm.Parameters.AddWithValue("@filename", file.FileName);
            sqlComm.Parameters.AddWithValue("@mime", System.IO.Path.GetExtension(file.FileName));
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();


        }
    }
}
