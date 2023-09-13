using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerWpf.Models
{
    public class Occupation
    {
        public string Uid { get; set; }
        public string ActivityId { get; set; }
        public string Activity { get; set; }
        public string EmployeeId { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

    }
}
