using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerWpf.Models
{
    public class Activity
    {
        public string Uid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
