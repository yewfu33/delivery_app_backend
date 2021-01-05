using AutoMapper;
using CsvHelper;
using Delivery_app.Entities;
using Delivery_app.Models;
using Delivery_app.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public ReportController(AppDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> UserReport()
        {
            try
            {
                var users = await _context.users
                            .Include(_ => _.orders)
                            .ToListAsync();

                var userReportModel = users
                    .Select(u => new UserReportModel
                    {
                        name = u.name,
                        email = u.email,
                        phone_num = u.phone_num,
                        user_id = u.user_id,
                        created_at = u.created_at,
                        total_orders = u.orders.Count(),
                    })
                    .OrderByDescending(p => p.total_orders)
                    .ToList();

                return View(userReportModel);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }

        public async Task<IActionResult> DownloadUserReportCSVFile()
        {
            try
            {
                var users = await _context.users
                            .Include(_ => _.orders)
                            .ToListAsync();

                var userReportModel = users
                    .Select(u => new UserReportModel
                    {
                        name = u.name,
                        email = u.email,
                        phone_num = u.phone_num,
                        user_id = u.user_id,
                        created_at = u.created_at,
                        total_orders = u.orders.Count(),
                    })
                    .OrderByDescending(p => p.total_orders)
                    .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<UserReportModelMap>();
                    csv.WriteRecords(userReportModel);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "UserReport.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("UserReport");
            }
        }

        public async Task<IActionResult> OrderReport()
        {
            try
            {
                var orders = await _context.orders
                            .Include(o => o.drop_points)
                            .Include(o => o.user)
                            .ToListAsync();

                var orderReportModel = orders
                    .Select(async o => new OrderReportModel
                    {
                        order_id = o.order_id,
                        delivery_item = o.name,
                        username = o.user.name,
                        created_on = o.created_at,
                        delivery_status = o.delivery_status,
                        courier = (o.courier_id != 0) 
                            ? (await _context.couriers.FindAsync(o.courier_id)).name 
                            : "",
                    })
                    .Select(p => p.Result)
                    .ToList();


                return View(orderReportModel);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }

        public async Task<IActionResult> DownloadOrderReportCSVFile()
        {
            try
            {
                var orders = await _context.orders
                            .Include(o => o.drop_points)
                            .Include(o => o.user)
                            .ToListAsync();

                var orderReportModel = orders
                    .Select(async o => new OrderReportModel
                    {
                        order_id = o.order_id,
                        delivery_item = o.name,
                        username = o.user.name,
                        created_on = o.created_at,
                        delivery_status = o.delivery_status,
                        courier = (o.courier_id != 0)
                            ? (await _context.couriers.FindAsync(o.courier_id)).name
                            : "",
                    })
                    .Select(p => p.Result)
                    .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<OrderReportModelMap>();
                    csv.WriteRecords(orderReportModel);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "OrderReport.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("OrderReport");
            }
        }

        public async Task<IActionResult> CourierReport()
        {
            try
            {
                var couriers = await _context.couriers
                            .ToListAsync();

                var courierReportModel = couriers
                    .Select(async c => new CourierReportModel
                    {
                        courier_id = c.courier_id,
                        name = c.name,
                        email = c.email,
                        total_deliver = (await _context.orders.Where(o => o.courier_id == c.courier_id).ToListAsync()).Count(),
                        revenue = c.revenue,
                    })
                    .Select(p => p.Result)
                    .ToList();


                return View(courierReportModel);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }

        public async Task<IActionResult> DownloadCourierReportCSVFile()
        {
            try
            {
                var couriers = await _context.couriers
                            .ToListAsync();

                var courierReportModel = couriers
                    .Select(async c => new CourierReportModel
                    {
                        courier_id = c.courier_id,
                        name = c.name,
                        email = c.email,
                        total_deliver = (await _context.orders.Where(o => o.courier_id == c.courier_id).ToListAsync()).Count(),
                        revenue = c.revenue,
                    })
                    .Select(p => p.Result)
                    .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<CourierReportModelMap>();
                    csv.WriteRecords(courierReportModel);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "CourierReport.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("CourierReport");
            }
        }
    }
}
