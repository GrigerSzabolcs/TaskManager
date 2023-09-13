using TaskManager.Models;

namespace TaskManager.Logic
{
    public interface IActivityLogic
    {
        void Create(Activity activity);
        IEnumerable<Activity> Read();
    }
}