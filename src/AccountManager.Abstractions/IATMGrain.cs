using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Abstractions
{
    public interface IATMGrain : IGrainWithIntegerKey
    {
        [Transaction(TransactionOption.RequiresNew)]
        Task Transfer(Guid fromAccount, Guid toAccount, uint amountToTransfer);
    }
}
