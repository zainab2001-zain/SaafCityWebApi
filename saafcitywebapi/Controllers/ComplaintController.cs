using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web.Http.Results;
using saafcitywebapi.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Net.Http.Headers;

namespace saafcitywebapi.Controllers
{
    public class ComplaintController : ApiController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SaafCity_Database_2Entities"].ConnectionString;

        [HttpPost]
        [Route("api/Complaint/registercomplaint")]
        public IHttpActionResult registercomplaint(Complaint complaint)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Insert into Complaints values(@dateandtime,@location, @status,null, nULL, @email,NULL, NULL)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dateandtime", complaint.Complaint_Time);
                    command.Parameters.AddWithValue("@location", complaint.Complaint_Loction);
                    command.Parameters.AddWithValue("@status", complaint.Complaint_Status);
                    command.Parameters.AddWithValue("@email", complaint.Complainant_Email);

                    
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        // Create a JSON object with the "success" key and value
                        var response = new { success = true, status = "success", message = "Complaint Registered" };
                        return Ok(response);

                        // Return the JSON object with a 200 OK status code

                    }
                    else
                    {
                        // Create a JSON object with the "success" key and value
                        var response = new { success = false, status = "error", message = "Unable to register Complaint" };
                        return Ok(response);
                    }
                }
            }
        }

        [HttpPost]
        [Route("api/Complaint/uploadimage")]
        public IHttpActionResult uploadimage(Complaint complaint)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Complaints SET Complaint_Image = CONVERT(varbinary(max), @image) WHERE Complainant_Email = @email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Make sure complaint.Complaint_Image has the image data
                    command.Parameters.AddWithValue("@image", complaint.Complaint_Image);
                    command.Parameters.AddWithValue("@email", complaint.Complainant_Email);
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        // Create a JSON object with the "success" key and value
                        var response = new { success = true, status = "success", message = "Image uploaded successfully" };
                        return Ok(response);
                    }
                    else
                    {
                        // Create a JSON object with the "success" key and value
                        var response = new { success = false, status = "error", message = "Unable to upload image" };
                        return Ok(response);
                    }
                }
            }
        }

            [HttpPost]
            [Route("api/Complaint/upload")]
            public async Task<IHttpActionResult> UploadImage()
            {
                try
                {
                    // Get the logged-in user's email from the request headers
                    string userEmail = Request.Headers.GetValues("Complainant_Email").FirstOrDefault();

                    // Check if there is a file in the request
                    if (!Request.Content.IsMimeMultipartContent())
                    {
                        return BadRequest("No file found in the request.");
                    }

                    // Create a provider for reading multipart form data
                    var provider = new MultipartMemoryStreamProvider();
                    await Request.Content.ReadAsMultipartAsync(provider);

                    // Get the uploaded file from the provider
                    var file = provider.Contents.FirstOrDefault();

                    if (file != null)
                    {
                        // Read the file data as a byte array
                        var fileBytes = await file.ReadAsByteArrayAsync();

                        // Update the database record with the image data
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Update the record with the matching email
                            using (var command = new SqlCommand("UPDATE Complaints SET Complaint_Image = @ImageData WHERE Complainant_Email = @Email", connection))
                            {
                                command.Parameters.AddWithValue("@ImageData", fileBytes);
                                command.Parameters.AddWithValue("@Email", userEmail);

                           

                                int rowsAffected = command.ExecuteNonQuery();

                                connection.Close();

                                if (rowsAffected > 0)
                                {
                                    // Create a JSON object with the "success" key and value
                                    var response = new { success = true, status = "success", message = "Image uploaded successfully" };
                                    return Ok(response);
                                }
                                else
                                {
                                    // Create a JSON object with the "success" key and value
                                    var response = new { success = false, status = "error", message = "Unable to upload image" };
                                    return Ok(response);
                                }
                            }
                        }

                    
                    }
                    else
                    {
                        return BadRequest("No file found in the request.");
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }


        [HttpGet]
        [Route("api/Complaint/GetImage")]
        public IHttpActionResult GetImage()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the image data from the database based on the email
                    using (var command = new SqlCommand("SELECT Complaint_Image FROM Complaints WHERE Complaint_ID = @ID", connection))
                    {
                        command.Parameters.AddWithValue("@ID", "4057");

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read the image data as a byte array
                                byte[] imageBytes = (byte[])reader["Complaint_Image"];

                                // Create a MemoryStream from the byte array
                                MemoryStream memoryStream = new MemoryStream(imageBytes);

                                // Return the image as a HttpResponseMessage
                                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                                response.Content = new StreamContent(memoryStream);
                                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                                return ResponseMessage(response);
                            }
                            else
                            {
                                // Image not found in the database
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }
}

