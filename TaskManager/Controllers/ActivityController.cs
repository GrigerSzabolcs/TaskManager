using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityLogic activityLogic;

        public ActivityController(IActivityLogic activityLogic)
        {
            this.activityLogic = activityLogic;
        }

        [HttpGet]
        public IEnumerable<Activity> GetActivities()
        {
            IEnumerable<Activity> result = activityLogic.Read();
            return result;
        }

        [HttpPost]
        public void Create([FromBody] Activity activity)
        {
            activityLogic.Create(activity);
        }

    }
}
