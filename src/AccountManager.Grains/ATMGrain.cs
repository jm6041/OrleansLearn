using AccountManager.Abstractions;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Grains
{
    [StatelessWorker]
    public class ATMGrain : Grain, IATMGrain
    {
        Task IATMGrain.Transfer(Guid fromAccount, Guid toAccount, uint amountToTransfer)
        {
            return Task.WhenAll(
                this.GrainFactory.GetGrain<IAccountGrain>(fromAccount).Withdraw(amountToTransfer),
                this.GrainFactory.GetGrain<IAccountGrain>(toAccount).Deposit(amountToTransfer));
        }
    }
}
