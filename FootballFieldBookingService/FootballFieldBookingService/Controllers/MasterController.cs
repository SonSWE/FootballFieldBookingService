using CommonLib;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ObjectInfo;
using DataAccess.Base;
using FootballFieldBookingService.Helper;
using FootballFieldBookingService.Memory;
using Microsoft.AspNetCore.Authorization;

namespace BOShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataBaseController<T> : ControllerBase where T : MasterDataBase, new()
    {
        public string FunctionId => GetApiAuthorizeFunctionConfig()?.FunctionId ?? string.Empty;

        public readonly IDataAccess<T> DataAccess;


        public MasterDataBaseController(IDataAccess<T> dataAccess)
        {
            DataAccess = dataAccess;
        }

        //
        [Authorize]
        [ApiAuthorize(Action = AuthenAction.DETAIL)]
        [HttpGet("getdetailbyid")]
        public virtual async Task<T?> GetDetailById([FromQuery] decimal value)
        {
            var requestId = Utils.GenGuidStringN();
            var requestTime = DateTime.Now;
            var clientInfo = Request.GetClientInfo();
            //
            T? response = null;

            try
            {
                response = await Task.Run(() => DataAccess.GetById(requestId, value));

                if (response == null)
                {
                    Response.StatusCode = StatusCodes.Status204NoContent;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, $"[{requestId}] {ex.Message}");

                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            Logger.log.Info(JsonHelper.Serialize(new
            {
                requestId,
                requestTime,
                responseTime = DateTime.Now,
                processTime = ConstLog.GetProcessingMilliseconds(requestTime),
                clientInfo,
                request = new { value },
                response
            }));

            return response;
        }

        // Create
        [Authorize]
        [ApiAuthorize(Action = AuthenAction.INSERT)]
        [HttpPost("insert")]
        public virtual async Task<MasterDataBaseBusinessResponse> Create([FromBody] T data)
        {
            var requestId = Utils.GenGuidStringN();
            var requestTime = DateTime.Now;
            var clientInfo = Request.GetClientInfo();

            MasterDataBaseBusinessResponse response = new();

            try
            {
                var result = await Task.Run(() =>
                {
                    var createResult = DataAccess.Insert(requestId, data);
                    return new Tuple<long>(createResult);
                });

                //
                response.Code = result.Item1;
                response.Message = DefErrorMem.GetErrorDesc(result.Item1);

                if (response.Code <= 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, $"[{requestId}] {ex.Message}");

                response.Code = ErrorCodes.Err_Exception;
                response.Message = ex.Message;

                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            Logger.log.Info(JsonHelper.Serialize(new
            {
                requestId,
                requestTime,
                responseTime = DateTime.Now,
                processTime = ConstLog.GetProcessingMilliseconds(requestTime),
                clientInfo,
                request = data,
                response
            }));

            return response;
        }

        // Update
        [Authorize]
        [ApiAuthorize(Action = AuthenAction.UPDATE)]
        [HttpPut("update")]
        public virtual async Task<MasterDataBaseBusinessResponse> Update([FromBody] T data)
        {
            var requestId = Utils.GenGuidStringN();
            var requestTime = DateTime.Now;
            var clientInfo = Request.GetClientInfo();

            //
            MasterDataBaseBusinessResponse response = new();

            try
            {
                var result = await Task.Run(() =>
                {
                    var createResult = DataAccess.Update(requestId, data);
                    return new Tuple<long>(createResult);
                });

                //
                response.Code = result.Item1;
                response.Message =  DefErrorMem.GetErrorDesc(result.Item1);

                if (response.Code <= 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, $"[{requestId}] {ex.Message}");

                response.Code = ErrorCodes.Err_Exception;
                response.Message = ex.Message;

                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            Logger.log.Info(JsonHelper.Serialize(new
            {
                requestId,
                requestTime,
                responseTime = DateTime.Now,
                processTime = ConstLog.GetProcessingMilliseconds(requestTime),
                clientInfo,
                request = data,
                response
            }));

            return response;
        }

        // Delete
        [Authorize]
        [ApiAuthorize(Action = AuthenAction.DELETE)]
        [HttpDelete("delete")]
        public virtual async Task<MasterDataBaseBusinessResponse> Delete([FromBody] decimal value)
        {
            var requestId = Utils.GenGuidStringN();
            var requestTime = DateTime.Now;
            var clientInfo = Request.GetClientInfo();

            MasterDataBaseBusinessResponse response = new();

            try
            {
                var result = await Task.Run(() => DataAccess.Delete(requestId, value));

                response.Code = result;
                response.Message = DefErrorMem.GetErrorDesc(result);

                if (result <= 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, $"[{requestId}] {ex.Message}");

                response.Code = ErrorCodes.Err_Exception;
                response.Message = DefErrorMem.GetErrorDesc(ErrorCodes.Err_Exception);

                Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            Logger.log.Info(JsonHelper.Serialize(new
            {
                requestId,
                requestTime,
                responseTime = DateTime.Now,
                processTime = ConstLog.GetProcessingMilliseconds(requestTime),
                clientInfo,
                request = value,
                response
            }));

            return response;
        }

        // Private Funcs
        #region Private Funcs

        private ApiAuthorizeFunctionConfigAttribute? GetApiAuthorizeFunctionConfig()
        {
            var attributes = this.GetType().GetCustomAttributes(typeof(ApiAuthorizeFunctionConfigAttribute), true);

            if (attributes != null && attributes.Length > 0)
            {
                return (ApiAuthorizeFunctionConfigAttribute)attributes[0];
            }

            return null;
        }

        #endregion
    }
}
