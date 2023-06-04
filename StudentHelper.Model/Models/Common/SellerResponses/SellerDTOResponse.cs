using StudentHelper.Model.Models.Entities.SellerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentHelper.Model.Models.Common.SellerResponses
{
    public class SellerDTOResponse : Response
    {
        public SellerDTO? SellerDTO { get; set; }
        public SellerDTOResponse(int statusCode, bool success, string message, SellerDTO sellerDTO) : base(statusCode, success, message)
        {
            SellerDTO = sellerDTO;
        }
    }
}
