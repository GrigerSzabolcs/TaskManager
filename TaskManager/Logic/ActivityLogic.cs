using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Logic
{
    public class ActivityLogic : IActivityLogic
    {
        IActivityRepository activityRepo;
        public ActivityLogic(IActivityRepository activityRepo)
        {
            this.activityRepo = activityRepo;
        }

        public void Create(Activity activity)
        {
            activityRepo.Create(activity);
        }

        public IEnumerable<Activity> Read()
        {
            return activityRepo.Read();
        }
    }
}
