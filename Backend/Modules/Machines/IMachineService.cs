﻿using Backend.Modules.Machines.Ent;

namespace Backend.Modules.Machines
{
    public interface ILoginService
    {
        Task<List<Machine>> GetMachines();
    }
}
