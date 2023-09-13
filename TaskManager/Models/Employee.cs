using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TaskManager.Models
{
    public class Employee : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [NotMapped]
        //[IgnoreDataMember]
        [JsonIgnore]
        public virtual ICollection<Occupation> Occupations { get; set; }

        public Employee()
        {
            Occupations = new HashSet<Occupation>();
        }

    }
}
