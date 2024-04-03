using System;
using System.Linq;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        public readonly Guid Guid;

        public Enterprise(Guid guid)
        {
            this.Guid = guid;
        }

        public string Name { get; set; }
        private string inn;

        public string Inn
        {
            get => inn;
            set => ValidateAndSetINN(value);
        }

        public DateTime EstablishDate { get; set; }

        public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

        public double GetTotalTransactionsAmount()
        {
            var transactions = DataBase.Transactions().Where(t => t.EnterpriseGuid == Guid);
            return transactions.Sum(t => t.Amount);
        }

        private void ValidateAndSetINN(string value)
        {
            if (value.Length != 10 || !value.All(char.IsDigit))
                throw new ArgumentException("Invalid INN length or format.");

            inn = value;
        }
    }
}