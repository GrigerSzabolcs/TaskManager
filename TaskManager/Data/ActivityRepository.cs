using TaskManager.Models;

namespace TaskManager.Data
{
    public class ActivityRepository : IActivityRepository
    {
        ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public void Create(Activity activity)
        {
            _context.Activities.Add(activity);
            _context.SaveChanges();
        }

        public IEnumerable<Activity> Read()
        {
            return _context.Activities;
        }
    }
}
