using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaskManager.Models
{
    public class Activity
    {
        [Key]
        public string Uid { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        [NotMapped]
        //[IgnoreDataMember]
        [JsonIgnore]
        public virtual ICollection<Occupation> Occupations { get; set; }

        public Activity()
        {
            Uid = Guid.NewGuid().ToString();
            Occupations = new HashSet<Occupation>();
        }
    }
}
