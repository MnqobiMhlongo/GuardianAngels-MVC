using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GuardianAngels.Models
{
    public class Schoolregister
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int registerId { get; set; }

        public DateTime Date { get; set; }

        public int studentid { get; set; }
        public string studentName { get; set; }
        public string studentImage { get; set; }
        public string Status { get; set; }
    }
}