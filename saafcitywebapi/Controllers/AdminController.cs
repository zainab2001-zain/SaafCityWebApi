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
    public class AdminController : ApiController
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SaafCity_Database_2Entities"].ConnectionString;

        [HttpPost]
        [Route("api/Admin/login")]
        public IHttpActionResult Login([FromBody] Admin admin)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Admins WHERE Employee_Id = @Employee_Id AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Employee_Id", admin.Employee_Id);
                        command.Parameters.AddWithValue("@Password", admin.Password);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            var response = new { success = true, status = "success", message = "valid user" };
                            return Ok(response);
                        }
                        else
                        {
                            var response = new { success = false, status = "error", message = "Invalid user" };
                            return Ok(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or return an error response
                var response = new { success = false, status = "error", message = ex.Message };
                return Ok(response);
            }
        }
    }
}
