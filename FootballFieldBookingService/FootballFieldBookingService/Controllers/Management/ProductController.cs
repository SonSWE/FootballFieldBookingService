using BOShare.Controllers;
using CommonLib;
using DataAccess.Base;
using FootballFieldBookingService.Helper;
using Microsoft.AspNetCore.Mvc;
using ObjectInfo;

namespace FootballFieldBookingService.Controllers.Management
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorizeFunctionConfig(AuthenFunctionId.PRODUCT)]
    public class ProductController : MasterDataBaseController<ProductInfo>
    {
        private readonly IDataAccess<ProductInfo> _dataAccess;
        public ProductController(IDataAccess<ProductInfo> dataAccess) : base(dataAccess)
        {
            _dataAccess = dataAccess;
        }
    }
}
