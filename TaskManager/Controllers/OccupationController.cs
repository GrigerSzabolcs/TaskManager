using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OccupationController : ControllerBase
    {
        private readonly IOccupationLogic occupationLogic;
        private readonly UserManager<Employee> userManager;

        public OccupationController(UserManager<Employee> userManager, IOccupationLogic occupationLogic)
        {
            this.occupationLogic = occupationLogic;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        //Gets the occupacions of the logged in employee
        public IEnumerable<Occupation> GetOccupations()
        {
            var user = userManager.Users.FirstOrDefault(t => t.UserName == this.User.Identity.Name);
            return user.Occupations;
        }

        [HttpPost]
        public void Create([FromBody] Occupation occupation)
        {
            occupationLogic.Create(occupation);
        }
    }
}
