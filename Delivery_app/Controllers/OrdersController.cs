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
using Microsoft.AspNetCore.Http;

namespace Delivery_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orders;
        private readonly AppDbContext _context;

        public OrdersController(IOrderService orders, AppDbContext context)
        {
            this._orders = orders;
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderModel>>> Getorders()
        {
            try
            {
                IEnumerable<OrderModel> orders = await _orders.GetAllOrders();
                return orders.ToList();
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
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

        [HttpGet("users/active/{id}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetActiveOrders(int id)
        {
            try
            {
                IEnumerable<OrderModel> orders = await _orders.GetActiveOrders(id);

                if (orders == null)
                {
                    return NotFound();
                }

                return orders.ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet("users/completed/{id}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetCompletedOrders(int id)
        {
            try
            {
                IEnumerable<OrderModel> orders = await _orders.GetCompletedOrders(id);

                if (orders == null)
                {
                    return NotFound();
                }

                return orders.ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
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
            try
            {
                if (order != null)
                {
                    await _orders.AddOrder(order);
                }
                else
                {
                    return BadRequest();
                }

                return CreatedAtAction("GetOrders", new { id = order.order_id });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost("take/{order_id}")]
        public async Task<ActionResult> TakeOrder(int order_id, [FromQuery] int courier_id)
        {
            try
            {
                await _orders.TakeOrder(order_id, courier_id);

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
                var orders = await _orders.FetchCourierTask(courier_id, status);

                return Ok(orders);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
            }
        }

        [HttpPost("promocode")]
        public async Task<ActionResult> PromoCode(ApplyPromoCodeModel model)
        {
            try
            {
                var promoCode = await _context.promo_codes.FirstOrDefaultAsync(p => p.name == model.promo_code);

                if(promoCode == null)
                {
                    return NotFound(new { message = "No promo code found" });
                }

                if(model.order_fee < promoCode.minimum_spend)
                {
                    return BadRequest(new { message = $"Minimum spend RM{promoCode.minimum_spend} required for this promo code" });
                }

                if(promoCode.quantity == 0)
                {
                    return BadRequest(new { message = "Promo code out of claim quantity" });
                }

                var checkValidity = DateTime.Compare(DateTime.Now, promoCode.validity);
                if(checkValidity > 0)
                {
                    return BadRequest(new { message = "Promo code is expired" });
                }

                var discount = 0.0;
                if(promoCode.discount_type == DiscountType.Percent)
                {
                    discount = (model.order_fee * promoCode.discount) / 100;
                }
                else if(promoCode.discount_type == DiscountType.Value)
                {
                    discount = promoCode.discount;
                }

                return Ok(new { 
                    id = promoCode.promo_code_id,
                    discount = discount
                });
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
            }
        }

        [HttpPost("payment")]
        public async Task<ActionResult> Payment(AddPaymentModel model)
        {
            try
            {
                await _orders.OrderPayment(model);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost("search")]
        public async Task<ActionResult> SearchOrders([FromQuery] string query)
        {
            try
            {
                return Ok(await _orders.searchOrders(query));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPost("cancel/{order_id}")]
        public async Task<ActionResult> CancelOrder(int order_id)
        {
            try
            {
                await _orders.CancelOrder(order_id);
                return Ok();
            }
            catch(Exception e)
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
