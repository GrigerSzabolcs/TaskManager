using TaskManager.Models;

namespace TaskManager.Logic
{
    public interface IOccupationLogic
    {
        void Create(Occupation occupation);
        IEnumerable<Occupation> Read();
        IEnumerable<Occupation> Read(string employeeId);
    }
}