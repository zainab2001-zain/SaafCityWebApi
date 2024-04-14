using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saafcitywebapi.Models
{
    public class Complainant
    {
        public int Complainant_ID { get; set; }
        public string Complainant_Name { get; set; }
        public string Complainant_Email { get; set; }
        public string Complainant_PhoneNo { get; set; }
        public string Complainant_Password { get; set; }
        public string New_Password { get; set; }
        public Nullable<int> Complaint_ID { get; set; }
        public string Address { get; set; }
        public Complaint Complaint { get; set; }
        public DateTime Date_Of_Birth { get; set; }
    }
}