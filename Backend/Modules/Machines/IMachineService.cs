using Backend.Modules.Machines.Ent;

namespace Backend.Modules.Machines
{
    public interface IMachineService
    {
        Task<List<Machine>> GetMachines();
    }
}
