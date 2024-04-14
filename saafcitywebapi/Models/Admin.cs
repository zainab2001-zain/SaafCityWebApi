using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saafcitywebapi.Models
{
    public class Admin
    {
        public string User_ID { get; set; }
        public string Password { get; set; }
        public Nullable<int> Employee_Id { get; set; }

        
    }
}