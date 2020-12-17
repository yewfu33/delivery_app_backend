using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Delivery_app.Models;
using Delivery_app.Services;
using Microsoft.AspNetCore.Authorization;
using Delivery_app.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Delivery_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orders;

        public OrdersController(IOrderService orders)
        {
            this._orders = orders;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IEnumerable<OrderModel>> Getorders()
        {
            return await _orders.GetAllOrders();
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrder(int id)
        {
            OrderModel orders = await _orders.GetOrder(id);

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // GET: api/orders/users/5
        [HttpGet("users/{id}")]
        public ActionResult<IEnumerable<OrderModel>> GetOrderByUserId(int id)
        {
            IEnumerable<OrderModel> orders = _orders.GetOrderByUserId(id);

            if (orders == null)
            {
                return NotFound();
            }

            return orders.ToList();
        }

        // PUT: api/orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(int id, Orders order)
        {
            if (id != order.order_id)
            {
                return BadRequest();
            }

            try
            {
                await _orders.EditOrder(id, order);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostOrder(Orders order)
        {
            if (order != null)
            {
                await _orders.AddOrder(order);
            } else
            {
                return BadRequest();
            }

            return CreatedAtAction("GetOrders", new { id = order.order_id });
        }

        [HttpPost("take/{order_id}")]
        public async Task<ActionResult> TakeOrder(int order_id, [FromQuery] int courier_id)
        {
            try
            {
                await _orders.takeOrder(order_id, courier_id);

                return Ok();
            }catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
            }
        }

        [HttpPost("status/update/{order_id}")]
        public async Task<ActionResult> UpdateStatus(int order_id, [FromQuery] int status)
        {
            try
            {
                int s = await _orders.updateStatus(order_id, status);

                return Ok(s);
            }catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
            }
        }

        [HttpPost("task/courier/{courier_id}")]
        public async Task<ActionResult> CheckCourierTask(int courier_id, [FromQuery] int status)
        {
            try
            {
                var orders = await _orders.fetchCourierTask(courier_id, status);

                return Ok(orders);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
            }
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderModel>> DeleteOrder(int id)
        {
            OrderModel order = await _orders.DeleteOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        private bool OrderExists(int id)
        {
            return _orders.OrderExist(id);
        }
    }
}
