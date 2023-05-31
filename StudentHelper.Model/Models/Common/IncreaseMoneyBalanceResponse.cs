using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common
{
    public class IncreaseMoneyBalanceResponse : Response
    {
        public IncreaseMoneyBalanceResponse(int statusCode, bool success, string message, decimal moneyAmount) : base(statusCode, success, message)
        {
            MoneyAmount = moneyAmount;
        }

        public decimal MoneyAmount { get; set; }

    }
}
