using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication4
{
    /// <summary>
    /// Descrizione di riepilogo per PdfHandler
    /// </summary>
    public class PdfHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            // Get the ID from the query string
            string hardwareId = context.Request.QueryString["id"];

            if (!string.IsNullOrEmpty(hardwareId))
            {
                int fileId = -1;

                if (hardwareId.ToString() != "")
                {
                    string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["WebSiteConnectionString"].ConnectionString;

                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM FILES WHERE FILE_IdHardware=@FILE_IdHardware", conn))
                        {
                            cmd.Parameters.AddWithValue("@FILE_IdHardware", hardwareId.ToString());
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                fileId = Convert.ToInt32(result);
                            }
                        }

                        string query = "SELECT FILE_B64 FROM FILES WHERE FILE_Id=@FILE_Id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@FILE_Id", fileId.ToString());
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                byte[] pdfBytes = result as byte[];
                                // Set the response headers to indicate it's a PDF file
                                context.Response.ContentType = "application/pdf";
                                context.Response.AddHeader("Content-Disposition", "inline; filename=pdfFile.pdf");

                                // Write the PDF bytes to the response
                                context.Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                                context.Response.End();
                            }
                            else
                            {
                                context.Response.StatusCode = 404;
                                context.Response.Write("PDF not found.");
                            }
                        }
                    }     
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Invalid ID.");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }    
}
