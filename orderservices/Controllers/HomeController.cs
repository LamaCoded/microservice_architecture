using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orderservices.data.model;
using orderservices.data.repo;
using userservice.helper;

namespace orderservices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly orderRepo _orderRepo;

        public OrderController(orderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetOrders(int? id)
        {
            var result = _orderRepo.GetOrders(id);
            return Ok(DbHelper.ToJson(result));
        }

        [Authorize]
        [HttpPost]
        public IActionResult SaveOrder([FromBody] OrderModel order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Invalid token");

            var result = _orderRepo.SaveOrders(
               int.Parse(userId),
                order.ProductId,
                order.Price,
                order.CreatedDate
            );

            return Ok(new
            {
                Message = "Order saved successfully",
                UserId = userId,
                Username = username,
                Data = result
            });
        }
    }
}
