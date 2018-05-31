using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfomationManager.Models;
using Orleans;

namespace InfomationManager.Abstractions
{
    public interface ISystemUserGrain : IGrainWithGuidKey
    {
        Task Add(SystemUser user);
        Task<List<SystemUser>> Get();
    }
}
