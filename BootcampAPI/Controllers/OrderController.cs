using BootcampAPI.Data;
using BootcampAPI.Models;
using BootcampAPI.Models.Dto;
using BootcampAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BootcampAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
            _response = new();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId,
            string searchString, string status, int pageNumber =1 , int pageSize =5)
        {
            try
            {
                IEnumerable<OrderHeader> orderHeader =
                    _db.OrderHeaders.Include(u => u.OrderDetails).
                    ThenInclude(u => u.MenuItem).
                    OrderByDescending(u => u.OrderHeaderId);


                if (!string.IsNullOrEmpty(userId))
                {
                    _response.Result = orderHeader.Where(u => u.ApplicationUserId == userId);
                }

                if (!string.IsNullOrEmpty(searchString))
                    {
                    orderHeader = orderHeader.Where(u =>
                    u.PickupPhoneNumber.ToLower().Contains(searchString) ||
                    u.PickupEmail.ToLower().Contains(searchString) ||
                    u.PickupName.ToLower().Contains(searchString)
                    );
                     }
                if (!string.IsNullOrEmpty(status))
                {
                    orderHeader = orderHeader.Where(u => u.Status.ToLower() == status.ToLower());
                }
                Pagination pagination = new Pagination()
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = orderHeader.Count(),
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = orderHeader.Skip((pageNumber-1) *pageSize).Take(pageSize);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrders(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }


                var orderHeader = _db.OrderHeaders.Include(u => u.OrderDetails).
                    ThenInclude(u => u.MenuItem).
                    Where(u => u.OrderHeaderId == id);
                if (orderHeader == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = orderHeader;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto orderHeaderDto)
        {
            try
            {
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDto.ApplicationUserId,
                    PickupEmail = orderHeaderDto.PickupEmail,
                    PickupPhoneNumber = orderHeaderDto.PickupPhoneNumber,
                    PickupName = orderHeaderDto.PickupName,
                    OrderTotal = orderHeaderDto.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentId = orderHeaderDto.StripePaymentIntentId,
                    TotalItems = orderHeaderDto.TotalItems,
                    Status = String.IsNullOrEmpty(orderHeaderDto.Status) ? SD.status_pending : orderHeaderDto.Status,
                };
                if (ModelState.IsValid)
                {
                    _db.OrderHeaders.Add(order);
                    _db.SaveChanges();
                    foreach (var orderDetailDto in orderHeaderDto.OrderDetailsDTO)
                    {
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            ItemName = orderDetailDto.ItemName,
                            MenuItemId = orderDetailDto.MenuItemId,
                            Price = orderDetailDto.Price,
                            Quantity = orderDetailDto.Quantity
                        };
                        _db.OrderDetails.Add(orderDetails);
                    }
                    _db.SaveChanges();
                    _response.Result = order;
                    order.OrderDetails = null;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateOrderHeader(int id, [FromBody] OrderHeaderUpdateDto orderHeaderUpdate)
        {
            try
            {
                if (orderHeaderUpdate == null || id != orderHeaderUpdate.OrderHeaderId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                OrderHeader orderFrDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == id);
                if(orderFrDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.PickupName))
                {
                    orderFrDb.PickupName = orderHeaderUpdate.PickupName;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.PickupPhoneNumber))
                {
                    orderFrDb.PickupPhoneNumber = orderHeaderUpdate.PickupPhoneNumber;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.PickupEmail))
                {
                    orderFrDb.PickupEmail = orderHeaderUpdate.PickupEmail;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.Status))
                {
                    orderFrDb.Status = orderHeaderUpdate.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdate.StripePaymentIntentId))
                {
                    orderFrDb.StripePaymentIntentId = orderHeaderUpdate.StripePaymentIntentId;
                }
                _db.SaveChanges();
                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }


}
