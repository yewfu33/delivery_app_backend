using AutoMapper;
using CsvHelper;
using Delivery_app.Entities;
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

        public async Task<IActionResult> Index()
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

        public async Task<IActionResult> DownloadCSVFile()
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
                return File(stream, "application/octet-stream", "UserReports.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("index");
            }
        }
    }
}
