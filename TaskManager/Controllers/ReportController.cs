using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TaskManager.Logic;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportController : ControllerBase
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IOccupationLogic occupationLogic;

        public ReportController(UserManager<Employee> userManager, IOccupationLogic occupationLogic)
        {
            _userManager = userManager;
            this.occupationLogic = occupationLogic;
        }

        [HttpPost]
        public IEnumerable<KeyValuePair<string, int>> GetMyReport([FromBody]Interval period)
        {

            //Returns the time, the employee spent with a task within a given perid.
            var employee = _userManager.Users.FirstOrDefault(t => t.UserName == this.User.Identity.Name);

            //var userOcc = occupationLogic.Read(employee.Id);

            //userOcc = userOcc.Where(o => (o.Date >= period.From && o.Date <= period.To));

            var userOcc = occupationLogic.Read(employee.Id).Where(o => (o.Date >= period.From && o.Date <= period.To));
            return userOcc
                .GroupBy(x => x.Activity.Title)
                .Select(x => new KeyValuePair<string, int>
                (x.Key, x.Sum(a => (a.Hour * 60 + a.Minute))
                ));
        }

        [HttpPost]
        public IEnumerable<KeyValuePair<string, int>> GetBigReport([FromBody] Interval period)
        {
            //Returns the time, the company spent with a task within a given perid.
            var Occ = occupationLogic.Read().Where(o => (o.Date >= period.From && o.Date <= period.To));
            return Occ
                .GroupBy(x => x.Activity.Title)
                .Select(x => new KeyValuePair<string, int>
                (x.Key, x.Sum(a => (a.Hour * 60 + a.Minute))
                ));
        }


    }
}
