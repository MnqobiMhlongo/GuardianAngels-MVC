using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GuardianAngels.Models
{
    public class Student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string StudentImage { get; set; }
        public int SchoolId { get; set; }
        public School School { get; set; }
        public string ContactNumber { get; set; }
        public string ParentEmail { get; set; }
        public string QR { get; set; }
        public string Status { get; set; }
        public int vId { get; set; }

    }
}