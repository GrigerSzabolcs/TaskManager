using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Logic
{
    public class OccupationLogic : IOccupationLogic
    {
        IOccupationRepository occupationRepo;
        public OccupationLogic(IOccupationRepository occupationRepo)
        {
            this.occupationRepo = occupationRepo;
        }

        public void Create(Occupation occupation)
        {
            occupationRepo.Create(occupation);
        }

        public IEnumerable<Occupation> Read()
        {
            return occupationRepo.Read().ToList();
        }

        public IEnumerable<Occupation> Read(string employeeId)
        {
            return occupationRepo.Read().Where(o => o.EmployeeId == employeeId).ToList();
        }
    }
}
