using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaskManager.Models
{
    public class Occupation
    {
        [Key]
        public string Uid { get; set; }
        public string ActivityId { get; set; }
        public string EmployeeId { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        [JsonIgnore]
        public virtual Employee? Employee { get; set; }
        [NotMapped]
        [IgnoreDataMember]
        [JsonIgnore]
        public virtual Activity? Activity { get; set; }
        public Occupation()
        {
            Uid = Guid.NewGuid().ToString();
        }
    }
}
