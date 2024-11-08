using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GuardianAngels.Models
{
    public class Vehicle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }

        public string Name { get; set; }

        public string Plate {  get; set; }
        
        public string Color { get; set; }

        public string image { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

    }
}