using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using SMS_Services.Entity;
using SMS_Services.Model;
using SMS_Services.Repository;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace SMS_Services.Controllers
{
    [ApiController]
    [Route("api/SMS/")]
    public class SMSController : BaseController
    {
        private readonly ISMSRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SMSController(IHttpContextAccessor httpContextAccessor, ISMSRepository sMSRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            _repository = sMSRepository;

        }

        #region SMS
        [AllowAnonymous]
        [HttpGet("check-port")]
        public async Task<IActionResult> checkport()
        {
            _repository.check_port();
            return Ok();
        }
        [HttpGet("get-list-sms-by-phone")]
        public async Task<IActionResult> GetListSMSByPhone2(string phone_receive, string? phone_send)
        {
            return Ok(_repository.GetListSMSByPhone2(phone_receive, phone_send));
        }
        [HttpGet("get-list-sms-receive")]
        public async Task<IActionResult> GetListSMSReceive()
        {
            long user_id = userid(_httpContextAccessor);
            var response = await _repository.GetListSMSReceive(user_id);
            return Ok(new ResponseSingleContentModel<List<Message_Receive>>
            {
                StatusCode = 200,
                Message = "",
                Data = response
            });
        }
        [HttpGet("get-list-sms-receive-admin")]
        public async Task<IActionResult> GetListSMSReceiveAdmin()
        {
            var response = await _repository.GetListSMSReceiveAdmin();
            return Ok(new ResponseSingleContentModel<List<Message_Receive>>
            {
                StatusCode = 200,
                Message = "",
                Data = response
            });
        }
        [AllowAnonymous]
        [HttpPost("create-list-sms-receive")]
        public async Task<IActionResult> Create_SMS_Receive([FromBody] List<Message_Receive> model)
        {
            return Ok(_repository.Create_SMS_Receive(model));
        }
        [HttpGet("list-port")]
        public async Task<IActionResult> PortList()
        {
            return Ok(_repository.PortList());
        }
        [HttpGet("send-single-sms")]
        public async Task<IActionResult> SendSMSDirect(string phone_number, string message)
        {
            _repository.SendSMSDirect(phone_number, message);
            return Ok();
        }
        #endregion
        #region Order
        [HttpPost("order-create")]
        public async Task<IActionResult> OrderCreate([FromBody] Order model)
        {
            try
            {
                string customer_name = username(this._httpContextAccessor);
                long customer_id = userid(this._httpContextAccessor);
                bool customer_active = await this._repository.Customer_Check_Active(customer_name, customer_id);
                if (customer_active)
                {
                    var response = await this._repository.OrderCreate(model);
                    return Ok(new ResponseSingleContentModel<Order>
                    {
                        StatusCode = 200,
                        Message = "",
                        Data = response
                    });
                }
                else return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Tài khoản không tồn tại hoặc đã bị khoá",
                    Data = "Tài khoản không tồn tại hoặc đã bị khoá"
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpPost("order-modify")]
        public async Task<IActionResult> OrderModify([FromBody] Order model)
        {
            try
            {
                string customer_name = username(this._httpContextAccessor);
                long customer_id = userid(this._httpContextAccessor);
                bool customer_active = await this._repository.Customer_Check_Active(customer_name, customer_id);
                if (customer_active)
                {
                    var response = await this._repository.OrderModify(model);
                    if (response != null)
                    {
                        return Ok(new ResponseSingleContentModel<Order>
                        {
                            StatusCode = 200,
                            Message = "",
                            Data = response
                        });
                    }
                    else return Ok(new ResponseSingleContentModel<string>
                    {
                        StatusCode = 500,
                        Message = "Không tìm thấy đơn hàng hoặc đơn hàng đã được thực thi!",
                        Data = "Không tìm thấy đơn hàng hoặc đơn hàng đã được thực thi!"
                    });
                }
                else return Ok(new ResponseSingleContentModel<string>
                {
                    StatusCode = 500,
                    Message = "Tài khoản không tồn tại hoặc đã bị khoá",
                    Data = "Tài khoản không tồn tại hoặc đã bị khoá"
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpGet("order-list")]
        public async Task<IActionResult> OrderList()
        {
            try
            {
                var response = await this._repository.OrderList();
                return Ok(new ResponseSingleContentModel<List<Order>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = response
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        [HttpGet("order-detail")]
        public async Task<IActionResult> OrderDetail(long id)
        {
            try
            {
                var response = await this._repository.OrderDetail(id);
                return Ok(new ResponseSingleContentModel<Order>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = response
                });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        #endregion

        #region test
        [AllowAnonymous]
        [HttpPost("request")]
        public async Task<IActionResult> Request([FromBody] Data_Upload data)
        {

            try
            {
                var response = await this._repository.Request(data);
                if (response)
                {
                    return Ok(new ResponseSingleContentModel<bool>
                    {
                        StatusCode = 200,
                        Message = "",
                        Data = response
                    });
                }
                else
                    return Ok(new ResponseSingleContentModel<bool>
                    {
                        StatusCode = 500,
                        Message = "",
                        Data = response
                    });
            }
            catch (Exception)
            {
                return this.RouteToInternalServerError();
            }
        }
        #endregion
    }
}
