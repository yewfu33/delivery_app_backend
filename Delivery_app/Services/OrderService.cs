using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAllOrders();
        Task<OrderModel> GetOrder(int id);
        Task<IEnumerable<OrderModel>> GetActiveOrders(int id);
        Task<IEnumerable<OrderModel>> GetCompletedOrders(int id);
        Task AddOrder(Orders order);
        Task EditOrder(int id, Orders order);
        Task TakeOrder(int id, int courier_id);
        Task CancelOrder(int id);
        Task OrderPayment(AddPaymentModel model);
        Task<int> updateStatus(int id, int status);
        Task<OrderModel> DeleteOrder(int id);
        Task<List<OrderModel>> FetchCourierTask(int courier_id, int status);
        Task<List<OrderModel>> searchOrders(string query);
        bool OrderExist(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public OrderService(AppDbContext context, IMapper mapper, INotificationService notificationService)
        {
            this._context = context;
            this._mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task AddOrder(Orders order)
        {
            try
            {
                _context.orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<OrderModel> DeleteOrder(int id)
        {
            Orders order = await _context.orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }

            _context.orders.Remove(order);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderModel>(order);
        }

        public async Task EditOrder(int id, Orders order)
        {
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            try
            {
                List<Orders> orders = await _context.orders
                    .Where(o => o.courier_id == 0 && o.delivery_status == DeliveryStatus.Assigned)
                    .OrderByDescending(q => q.created_at)
                    .Include(o => o.drop_points)
                    .Include(o => o.user)
                    .ToListAsync();

                return _mapper.Map<List<Orders>, List<OrderModel>>(orders);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<OrderModel> GetOrder(int id)
        {
            Orders order = await _context.orders.FirstOrDefaultAsync(o => o.order_id == id);

            if (order == null)
            {
                return null;
            }
            else
            {
                _context.drop_points
                    .Where(dp => dp.order_id == order.order_id)
                    .Load();
                _context.users
                    .Where(u => u.user_id == order.user_id)
                    .Load();
            }

            return _mapper.Map<OrderModel>(order);
        }

        public async Task<IEnumerable<OrderModel>> GetActiveOrders(int id)
        {
            try
            {
                List<Orders> orders = await _context.orders
                            .Where(o => o.user_id == id)
                            .Where(o => ((int)o.delivery_status == 0) || ((int)o.delivery_status == 1))
                            .Include(o => o.drop_points)
                            .ToListAsync();

                if (orders == null)
                {
                    return null;
                }

                return _mapper.Map<List<Orders>, List<OrderModel>>(orders);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<OrderModel>> GetCompletedOrders(int id)
        {
            try
            {
                List<Orders> orders = await _context.orders
                            .Where(o => o.user_id == id)
                            .Where(o => ((int)o.delivery_status == 2) || ((int)o.delivery_status == 3))
                            .Include(o => o.drop_points)
                            .ToListAsync();

                if (orders == null)
                {
                    return null;
                }
                else
                {
                    var ordersModel = _mapper.Map<List<Orders>, List<OrderModel>>(orders);

                    foreach (var orderModel in ordersModel)
                    {
                        var courier = await _context.couriers.FindAsync(orderModel.courier_id);
                        orderModel.courier = _mapper.Map<Couriers, CourierModel>(courier);
                    }

                    return ordersModel;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool OrderExist(int id)
        {
            return _context.orders.Any(e => e.order_id == id);
        }

        public async Task TakeOrder(int id, int courier_id)
        {
            try
            {
                var order = await _context.orders.FindAsync(id);

                if (order == null)
                    throw new Exception("order no found");

                if (order.courier_id != 0)
                    throw new Exception("order already been taken");

                // set courier id
                order.courier_id = courier_id;
                _context.orders.Update(order);
                await _context.SaveChangesAsync();

                var user = await _context.users.FindAsync(order.user_id);

                // send notification
                await _notificationService.SendNotification(
                        user.fcm_token,
                        "Your Delivery Order has been assigned",
                        "Your Delivery Order has been assigned",
                        "You will get notifier when courier arrived the pick up"
                    );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> updateStatus(int id, int status)
        {
            try
            {
                var order = await _context.orders.FindAsync(id);

                if (order == null)
                    throw new Exception("order no found");

                // update order status
                order.delivery_status = (DeliveryStatus)status;
                _context.orders.Update(order);
                await _context.SaveChangesAsync();

                return (int)order.delivery_status;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<List<OrderModel>> FetchCourierTask(int courier_id, int status)
        {
            try
            {
                var orders = await _context.orders
                                .Where(o => o.courier_id == courier_id)
                                .Where(p => p.delivery_status == (DeliveryStatus)status)
                                .OrderByDescending(q => q.created_at)
                                .Include(o => o.drop_points)
                                .Include(o => o.user)
                                .ToListAsync();

                return _mapper.Map<List<Orders>, List<OrderModel>>(orders);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<OrderModel>> searchOrders(string query)
        {
            try
            {
                var orders = from o in _context.orders
                             select o;

                if (!String.IsNullOrEmpty(query))
                {
                    orders = orders
                            .Where(s =>
                                s.name.Contains(query)
                                || s.pick_up_address.Contains(query)
                                || s.comment.Contains(query))
                            .Include(d => d.drop_points);
                }

                return _mapper.Map<List<Orders>, List<OrderModel>>(await orders.ToListAsync());
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task CancelOrder(int id)
        {
            try
            {
                var order = await _context.orders.FindAsync(id);

                if(order == null) throw new Exception("Order no found");

                if(order.delivery_status != DeliveryStatus.Assigned)
                    throw new Exception("In delivery or completed order cannot be cancel, please contact support");

                // othherwise set order cancelled
                order.delivery_status = DeliveryStatus.Cancelled;

                _context.orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task OrderPayment(AddPaymentModel model)
        {
            try
            {
                var payment = _mapper.Map<Payments>(model);
                payment.created_at = DateTime.Now;
                await _context.payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
