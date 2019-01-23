using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PracticalTest.Models
{  
        public partial class CustomerViewModel
        {
            public int Id { get; set; }
        
            public string Name { get; set; }
      
            public string Phone { get; set; }

            public String Gender { get; set; }

            public String City { get; set; }

            public String Region { get; set; }

           [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
            public DateTime? LastPurchase { get; set; }

            public String Classification { get; set; }

            public String Seller { get; set; }
        }
 
}