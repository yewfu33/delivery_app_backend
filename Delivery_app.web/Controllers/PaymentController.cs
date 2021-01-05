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
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IToastNotification _toastNotification;

        public PaymentController(AppDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> OrderStatement()
        {
            try
            {
                var payments = await _context.payments
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.user)
                                .Include(p => p.courier)
                                .ToListAsync();

                var orderStatementModels = payments
                            .Select(p => new OrderStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                user = p.user.name,
                                courier = p.courier.name,
                                created_on = p.created_at,
                                order_amount = Math.Round(p.amount - p.courier_pay, 2, MidpointRounding.AwayFromZero),
                                payment_method = p.payment_method,
                                transaction_status = p.transaction_status,
                            })
                            .ToList();

                return View(orderStatementModels);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }

        public async Task<IActionResult> OrderSettlement()
        {
            try
            {
                var payments = await _context.payments
                                .Where(p => p.transaction_status == TransactionStatus.Unsettled)
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.user)
                                .Include(p => p.courier)
                                .ToListAsync();

                var orderStatementModels = payments
                            .Select(p => new OrderStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                user = p.user.name,
                                courier = p.courier.name,
                                created_on = p.created_at,
                                order_amount = Math.Round(p.amount - p.courier_pay, 2, MidpointRounding.AwayFromZero),
                                payment_method = p.payment_method,
                                transaction_status = p.transaction_status,
                            })
                            .ToList();

                return View(orderStatementModels);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }

        public async Task<IActionResult> CourierStatement()
        {
            try
            {
                var payments = await _context.payments
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.courier)
                                .ToListAsync();

                var courierStatementModels = payments
                            .Select(p => new CourierStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                courier = p.courier.name,
                                commission = p.courier.commission,
                                created_on = p.created_at,
                                order_amount = p.amount,
                                courier_pay = p.courier_pay,
                                payment_method = p.payment_method,
                                courier_payment_status = p.courier_payment_status,
                            })
                            .ToList();

                return View(courierStatementModels);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }
        public async Task<IActionResult> CourierSettlement()
        {
            try
            {
                var payments = await _context.payments
                                .Where(p => p.courier_payment_status == CourierPaymentStatus.Unsettled)
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.courier)
                                .ToListAsync();

                var courierStatementModels = payments
                            .Select(p => new CourierStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                courier = p.courier.name,
                                commission = p.courier.commission,
                                created_on = p.created_at,
                                order_amount = p.amount,
                                courier_pay = p.courier_pay,
                                payment_method = p.payment_method,
                                courier_payment_status = p.courier_payment_status,
                            })
                            .ToList();

                return View(courierStatementModels);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolvePayment(int[] settle_list)
        {
            try
            {
                if (settle_list.Length == 0)
                {
                    _toastNotification.AddErrorToastMessage($"Please select any of records to update.");
                    return RedirectToAction("OrderSettlement");
                }

                foreach (var id in settle_list)
                {
                    await UpdateTransaction(id);
                }

                _toastNotification.AddSuccessToastMessage($"Successfully resolve the payment.");

                return RedirectToAction("OrderSettlement");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to resolve payment, contact support.");
                return View("OrderSettlement");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayCourier(int[] settle_list)
        {
            try
            {
                if (settle_list.Length == 0)
                {
                    _toastNotification.AddErrorToastMessage($"Please select any of records to update.");
                    return RedirectToAction("CourierSettlement");
                }

                foreach (var id in settle_list)
                {
                    await UpdateCourierPayStatus(id);
                }

                _toastNotification.AddSuccessToastMessage($"Successfully paid to couriers.");

                return RedirectToAction("CourierSettlement");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to perform the action, contact support.");
                return View("CourierSettlement");
            }
        }

        private async Task UpdateTransaction(int id)
        {
            var payment = await _context.payments.FindAsync(id);
            payment.transaction_status = TransactionStatus.Received;
            _context.payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateCourierPayStatus(int id)
        {
            var payment = await _context.payments.FindAsync(id);
            payment.courier_payment_status = CourierPaymentStatus.Paid;
            _context.payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> DownloadOrderStatementCSVFile()
        {
            try
            {
                var payments = await _context.payments
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.user)
                                .Include(p => p.courier)
                                .ToListAsync();

                var orderStatementModels = payments
                            .Select(p => new OrderStatementModel
                            {
                                order_id = p.order_id,
                                user = p.user.name,
                                courier = p.courier.name,
                                created_on = p.created_at,
                                order_amount = Math.Round(p.amount - p.courier_pay, 2, MidpointRounding.AwayFromZero),
                                payment_method = p.payment_method,
                                transaction_status = p.transaction_status,
                            })
                            .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<OrderStatementModelMap>();
                    csv.WriteRecords(orderStatementModels);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "OrderStatement.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("OrderStatement");
            }
        }

        public async Task<IActionResult> DownloadOrderSettlementCSVFile()
        {
            try
            {
                var payments = await _context.payments
                                .Where(p => p.transaction_status == TransactionStatus.Unsettled)
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.user)
                                .Include(p => p.courier)
                                .ToListAsync();

                var orderStatementModels = payments
                            .Select(p => new OrderStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                user = p.user.name,
                                courier = p.courier.name,
                                created_on = p.created_at,
                                order_amount = Math.Round(p.amount - p.courier_pay, 2, MidpointRounding.AwayFromZero),
                                payment_method = p.payment_method,
                                transaction_status = p.transaction_status,
                            })
                            .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<OrderStatementModelMap>();
                    csv.WriteRecords(orderStatementModels);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "OrderSettlement.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("OrderSettlement");
            }
        }

        public async Task<IActionResult> DownloadCourierStatementCSVFile()
        {
            try
            {
                var payments = await _context.payments
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.courier)
                                .ToListAsync();

                var courierStatementModels = payments
                            .Select(p => new CourierStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                courier = p.courier.name,
                                commission = p.courier.commission,
                                created_on = p.created_at,
                                order_amount = p.amount,
                                courier_pay = p.courier_pay,
                                payment_method = p.payment_method,
                                courier_payment_status = p.courier_payment_status,
                            })
                            .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<CourierStatementModelMap>();
                    csv.WriteRecords(courierStatementModels);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "CourierStatement.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("CourierStatement");
            }
        }

        public async Task<IActionResult> DownloadCourierSettlementCSVFile()
        {
            try
            {
                var payments = await _context.payments
                                .Where(p => p.courier_payment_status == CourierPaymentStatus.Unsettled)
                                .OrderByDescending(p => p.created_at)
                                .Include(p => p.courier)
                                .ToListAsync();

                var courierStatementModels = payments
                            .Select(p => new CourierStatementModel
                            {
                                payment_id = p.payment_id,
                                order_id = p.order_id,
                                courier = p.courier.name,
                                commission = p.courier.commission,
                                created_on = p.created_at,
                                order_amount = p.amount,
                                courier_pay = p.courier_pay,
                                payment_method = p.payment_method,
                                courier_payment_status = p.courier_payment_status,
                            })
                            .ToList();

                // write to csv file
                var stream = new MemoryStream();
                using (var writeFile = new StreamWriter(stream, leaveOpen: true))
                {
                    var csv = new CsvWriter(writeFile, new System.Globalization.CultureInfo("en-US"));
                    csv.Configuration.RegisterClassMap<OrderStatementModelMap>();
                    csv.WriteRecords(courierStatementModels);
                }
                stream.Position = 0; //reset stream
                return File(stream, "application/octet-stream", "CourierSettlement.csv");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Failed to load the CSV file.");
                return RedirectToAction("CourierSettlement");
            }
        }
    }
}
