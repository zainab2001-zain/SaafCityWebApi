using saafcitywebapi.Models;
using System;
using System.Collections.Generic;

public class Complaint
{
    public int Complaint_ID { get; set; }
    public Nullable<System.DateTime> Complaint_Time { get; set; }
    public string Complaint_Loction { get; set; }
    public string Complaint_Status { get; set; }
    public Nullable<int> Depart_ID { get; set; }
    public string Comments { get; set; }
    public byte[] Complaint_Image { get; set; }
    public string Verification_Image { get; set; }
    public string Complainant_Email { get; set; }
    public ICollection<Complainant> Complainants { get; set; }
}
