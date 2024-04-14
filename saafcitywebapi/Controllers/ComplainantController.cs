using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using saafcitywebapi.Models;

namespace saafcitywebapi.Controllers
{
    public class ComplainantController : ApiController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SaafCity_Database_2Entities"].ConnectionString;

        [HttpPost]
        [Route("api/Complainant/getdetails")]
        public IHttpActionResult GetDetails(Complainant complainant)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select *from Complainnats where Complainant_Email=@complainant_email and Complainant_Password= @complainant_Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@complainant_email", complainant.Complainant_Email);
                    command.Parameters.AddWithValue("@complainant_Password", complainant.Complainant_Password);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            
                            complainant.Complainant_Email = (string)reader["Complainant_Email"];
                            complainant.Complainant_Password = (string)reader["Complainant_Password"];
                        }

                        reader.Close();
                        connection.Close();

                        return Ok(complainant);
                    }
                    else
                    {
                        reader.Close();
                        connection.Close();

                        return BadRequest("Invalid username or password");
                    }
                }
            }
        }

        [HttpPost]
        [Route("api/Complainant/login")]

        public IHttpActionResult login([FromBody] Complainant complainant)
        {
           
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select *from Complainnats where Complainant_Email= '" + complainant.Complainant_Email + "' and Complainant_Password= '" + complainant.Complainant_Password + "'",connectionString);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                var response = new { success = true, status = "success", message = "Data found" };
                return Ok(response);
            }
            else
            {
                var response = new { success = false,status = "", message = "Invalid user" };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("api/Complainant/updatePassword")]
        public IHttpActionResult UpdatePassword([FromBody] Complainant request)
        {
            // Retrieve the user's email, old password, and new password from the request body
            string email = request.Complainant_Email;
            string oldPassword = request.Complainant_Password;
            string newPassword = request.New_Password;

            // Check if the email and old password are correct in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("SELECT * FROM Complainnats WHERE Complainant_Email = @Email AND Complainant_Password = @OldPassword", connection);
                selectCommand.Parameters.AddWithValue("@Email", email);
                selectCommand.Parameters.AddWithValue("@OldPassword", oldPassword);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                DataTable dt = new DataTable();
                sqlDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Update the new password in the database
                    // Update the new password in the database
                    SqlCommand updateCommand = new SqlCommand("UPDATE Complainnats SET Complainant_Password = @NewPassword WHERE Complainant_Email = @Email", connection);
                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCommand.Parameters.AddWithValue("@Email", email);

                    updateCommand.ExecuteNonQuery();

                    var response = new { success = true, status = "success", message = "Password updated successfully." };
                    return Ok(response);
                }
                else
                {
                    var response = new { success = false, status = "", message = "Invalid email or old password." };
                    return Ok(response);
                }
            }
        }


        [HttpPost]
        [Route("api/Complainant/signup")]
        public IHttpActionResult Signup(Complainant complainant)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Complainnats (Complainant_Name,Complainant_Email,Complainant_PhoneNo,Complainant_Password,Address,Date_Of_Birth) VALUES (@name, @email, @phoneno, @password,@address,@dob)";

                Console.WriteLine("Name: " + complainant.Complainant_Name);
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", complainant.Complainant_Name);
                    command.Parameters.AddWithValue("@email", complainant.Complainant_Email);
                    command.Parameters.AddWithValue("@phoneno", complainant.Complainant_PhoneNo);
                    command.Parameters.AddWithValue("@password", complainant.Complainant_Password);
                    command.Parameters.AddWithValue("@address", complainant.Address);   
                    command.Parameters.AddWithValue("@dob", complainant.Date_Of_Birth);




                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        return Ok(complainant);
                    }
                    else
                    {
                        return BadRequest("Unable to create account");
                    }
                }
            }
        }
    }
}
