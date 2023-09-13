using TaskManager.Models;

namespace TaskManager.Data
{
    public class OccupationRepository : IOccupationRepository
    {
        ApplicationDbContext _context;

        public OccupationRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public void Create(Occupation occupation)
        {
            _context.Occupations.Add(occupation);
            _context.SaveChanges();
        }

        public IEnumerable<Occupation> Read()
        {
            return _context.Occupations;
        }
    }
}
