using Backend.Modules.Machines.Ent;

namespace Backend.Modules.Machines
{
    public interface MachineServiceI
    {
        Task<List<Machine>> GetMachines();
    }
}
