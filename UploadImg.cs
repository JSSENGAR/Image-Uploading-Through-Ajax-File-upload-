using System;
using CCLS;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using AjaxControlToolkit;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace NPage
{
    public partial class UploadImages : System.Web.UI.Page
    {
        MySqlC o = new MySqlC(); SqlConnection con = new SqlConnection(); string ConS = ""; string oprs = "";        
        byte[] imgByte = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ConS = o.sqlcs(); con.ConnectionString = ConS;  oprs = o.opr(Session["User_right"].ToString(), "~/NPages/UploadImages.aspx");
            Session["CP"] = "Update / Upload Images";            
        }
        protected void lgrid(string fn, string fname)
        {
            
        }
        protected void OnUploadComplete(object sender, AjaxFileUploadEventArgs e)
        {

            string contentTypes = e.ContentType.ToLower();
            string fileName = Path.GetFileName(e.FileName);
            string fn = "";
            
            AjaxFileUpload11.SaveAs(Server.MapPath("~/ScholarPhotos/" + fileName));
            string filePath = Server.MapPath("~/ScholarPhotos/" + fileName);

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);            
            for (int i = 0; fileName[i].ToString() != "."; i++)
                fn = fn+ fileName[i].ToString();


            string targetPath = Server.MapPath("~/Uploads/" + fileName);
           // Stream strm = fileupload1.PostedFile.InputStream;
            var targetFile = targetPath;
            //Based on scalefactor image size will vary
            GenerateThumbnails(fn, fs, targetFile);
         

        }

        private void GenerateThumbnails(string fn, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                 decimal sf =Convert.ToDecimal(336) /Convert.ToDecimal(image.Height);
                
                var newHeight = 336; //(int)(image.Height * scaleFactor);               
                var newWidth = (int)(image.Width * sf); //336;
                //var newHeight = (int)(image.Height * scaleFactor);


                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);



                FileStream fs = new FileStream(targetPath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((Int32)fs.Length);



                SqlCommand cmd = new SqlCommand("update SchReg set Stu_Pic = @Data where Adm_Scho=@IName");
                cmd.Parameters.AddWithValue("@IName", fn);
                cmd.Parameters.AddWithValue("@Data", bytes);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();                       

                
            }
        }
    }
   
}