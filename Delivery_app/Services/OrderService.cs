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
        IEnumerable<OrderModel> GetOrderByUserId(int id);
        Task AddOrder(Orders order);
        Task EditOrder(int id, Orders order);
        Task<OrderModel> DeleteOrder(int id);
        bool OrderExist(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task AddOrder(Orders order)
        {
            _context.orders.Add(order);
            await _context.SaveChangesAsync();
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
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            List<Orders> order = await _context.orders
                .Include(o=>o.drop_points)
                .Include(o=>o.user)
                .ToListAsync();

            return _mapper.Map<List<Orders>, List<OrderModel>>(order);
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

        public IEnumerable<OrderModel> GetOrderByUserId(int id)
        {
            List<Orders> order = _context.orders.Include(o => o.drop_points)
                            .Where(o=>o.user_id == id).ToList();

            if (order == null)
            {
                return null;
            }

            return _mapper.Map<List<Orders>, List<OrderModel>>(order);
        }

        public bool OrderExist(int id)
        {
            return _context.orders.Any(e => e.order_id == id);
        }
    }
}
