
using Backend.App.Machines.Ent;

namespace Backend.App.Machines
{
    public class MachineService: BaseService, MachineServiceI
    {
        public async Task<List<Machine>> GetMachines()
        {
            List <Machine> machines = new List<Machine>();
            await Sql.Run(
                    "SELECT * FROM _app.Machine m",
                    r => {
                        var m = ReadBaseEntity<Machine>(r);
                        m.StationPairs = GetInt(r, "stationPairs");
                        machines.Add(m);
                    }
            );
            return machines;
        }
    }
}
