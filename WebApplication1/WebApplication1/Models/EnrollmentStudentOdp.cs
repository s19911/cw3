using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenie3.Models
{
    public class EnrollmentStudentOdp
    {
        
        public string IndexNumber { get; set; }
        
        public String FirstName { get; set; }
       
        public String LastName { get; set; }
        
        public String BirthDate { get; set; }
        
        public string Studies { get; set; }
    }
}
