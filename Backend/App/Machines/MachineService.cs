using GC = Backend.GlobalConstants;
using Backend.App.Machines.Ent;

namespace Backend.App.Machines
{
    public class MachineService: BaseService, MachineServiceI
    {
        public MachineService(IServiceProvider serviceProvider) : base (serviceProvider) { }

        public async Task<List<Machine>> GetMachines(SessionEnt session)
        {
//DelaySeconds(3);

            List <Machine> machines = new List<Machine>();
            await Sql.Run(
                    "SELECT * FROM app.Machine m",
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
