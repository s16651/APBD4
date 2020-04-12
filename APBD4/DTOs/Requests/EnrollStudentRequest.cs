using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APBD4.DTOs.Requests
{
	public class EnrollStudentRequest
	{
        
        
        public string IndexNumber { get; set; }
        
        public string FirstName { get; set; } 
        
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Studies { get; set; }
    }
}
