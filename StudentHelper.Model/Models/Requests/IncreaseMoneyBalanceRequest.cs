using MediatR;
using StudentHelper.Model.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Requests
{
    public class IncreaseMoneyBalanceRequest : IRequest<IncreaseMoneyBalanceResponse>
    {
        [Required]
        public decimal MoneyAmount { get; set; }
    }
}
