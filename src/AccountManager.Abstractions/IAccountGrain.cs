using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Abstractions
{
    public interface IAccountGrain : IGrainWithGuidKey
    {
        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Transaction(TransactionOption.Required)]
        Task Withdraw(uint amount);
        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Transaction(TransactionOption.Required)]
        Task Deposit(uint amount);
        /// <summary>
        /// 余额
        /// </summary>
        /// <returns></returns>
        [Transaction(TransactionOption.Required)]
        Task<uint> GetBalance();
    }
}
