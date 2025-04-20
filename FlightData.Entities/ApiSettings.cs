using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightData.Entities
{
    public class ApiSettings
    {
        [Required(ErrorMessage = "The 'DataFilePath' field is required.")]
        public required string DataFilePath { get; set; }
    }
}
