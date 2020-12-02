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
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<OrderModel>> Getorders()
        {
            return await _orders.GetAllOrders();
        }

        // GET: api/orders/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrders(int id)
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
        public ActionResult<IEnumerable<OrderModel>> GetOrdersByUserId(int id)
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
                if (!OrdersExists(id))
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
        public async Task<ActionResult> PostOrders(Orders order)
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

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderModel>> DeleteOrders(int id)
        {
            OrderModel order = await _orders.DeleteOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        private bool OrdersExists(int id)
        {
            return _orders.OrderExist(id);
        }
    }
}
