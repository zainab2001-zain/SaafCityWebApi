using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace saafcitywebapi.Controllers
{
    public class ImagesController : ApiController
    {
        private readonly string _imageDirectory = "Images/";

        /*[HttpPost]
        public async Task<IActionResult> PostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image file received.");
            }

            // Generate a unique filename for the image
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Save the image file to the server
            using (var stream = new FileStream(_imageDirectory + filename, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return a success response with the filename of the saved image
            return Ok(new { filename });
        }*/
    }
}
