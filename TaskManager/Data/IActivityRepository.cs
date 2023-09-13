using TaskManager.Models;

namespace TaskManager.Data
{
    public interface IActivityRepository
    {
        void Create(Activity activity);
        IEnumerable<Activity> Read();
    }
}