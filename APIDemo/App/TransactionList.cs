using System;
using System.Collections.Generic;
using System.Transactions;

namespace APIDemo.App_Code
{
    public class TransactionList
    {
        public string ErrMsg { get; set; }
        private readonly List<ITransaction> Transactions;

        public TransactionList(List<ITransaction> transactions)
        {
            Transactions = transactions;
        }

        public bool ExecuteAll()
        {
            bool result = true;
            
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    foreach (var transaction in Transactions)
                    {
                        if (!transaction.Execute())
                        {
                            ErrMsg = transaction.ErrMsg;
                            return false;
                        }
                    }

                    tran.Complete();
                }
                catch (Exception)
                {
                    //TODO log
                    throw;
                }
            }

            return result;
        }
    }
}