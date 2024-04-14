using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace saafcitywebapi.Models
{
    public class dbcontext:DbContext
    {
        
            public dbcontext() : base("name=MyConnectionString")
            {
            }

            public DbSet<Complainant> Complainants { get; set; }
        }
    }

