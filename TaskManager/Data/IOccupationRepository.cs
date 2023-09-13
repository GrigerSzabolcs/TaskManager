using TaskManager.Models;

namespace TaskManager.Data
{
    public interface IOccupationRepository
    {
        void Create(Occupation occupation);
        IEnumerable<Occupation> Read();
    }
}