using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Controllers
{
    public class PromotionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public PromotionController(AppDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            try
            {
                var promoCodes = await _context.promo_codes
                                            .OrderByDescending(o => o.created_at)
                                            .ToListAsync();

                var promoCodesModel = _mapper.Map<List<PromoCodes>, List<PromoCodeModel>>(promoCodes);

                if (!String.IsNullOrEmpty(searchQuery))
                {
                    promoCodesModel = promoCodesModel.Where(o =>
                        o.name.Contains(searchQuery)
                        || o.description.Contains(searchQuery)
                        ).ToList();
                }

                return View(promoCodesModel);
            }
            catch(Exception)
            {
                _toastNotification.AddErrorToastMessage("Failed to fetch promo codes.");
                return View(null);
            }
        }

        public IActionResult Create()
        {
            return View(new PromoCodeModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromoCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var promoCode = _mapper.Map<PromoCodes>(model);

                promoCode.created_at = DateTime.Now;
                promoCode.updated_at = DateTime.Now;

                _context.promo_codes.Add(promoCode);

                await _context.SaveChangesAsync();

                _toastNotification.AddSuccessToastMessage("Promo Code added successfully.");

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Error occured, try again later.");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            var promoCode = await _context.promo_codes.FindAsync(id);

            if (promoCode == null)
            {
                _toastNotification.AddErrorToastMessage("No specified promo code found.");
                return RedirectToAction("Index");
            }

            var promoCodeModel = _mapper.Map<PromoCodeModel>(promoCode);

            return View(promoCodeModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PromoCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (id != model.promo_code_id)
            {
                ModelState.AddModelError("promo_code_id", "Id is not the same");
                return View(model);
            }

            try
            {
                if (id == 0)
                {
                    return RedirectToAction("Index");
                }

                var promoCode = await _context.promo_codes.FindAsync(id);

                if (promoCode == null)
                {
                    _toastNotification.AddErrorToastMessage("No found on this specified id.");
                    return RedirectToAction("Index");
                }

                var editPromoCode = _mapper.Map<PromoCodes>(model);

                // update fields
                promoCode.name = editPromoCode.name;
                promoCode.description = editPromoCode.description;
                promoCode.discount = editPromoCode.discount;
                promoCode.discount_type = editPromoCode.discount_type;
                promoCode.minimum_spend = editPromoCode.minimum_spend;
                promoCode.num_claim_per_user = editPromoCode.num_claim_per_user;
                promoCode.quantity = editPromoCode.quantity;
                promoCode.validity = editPromoCode.validity;
                promoCode.updated_at = DateTime.Now;

                _context.promo_codes.Update(promoCode);

                await _context.SaveChangesAsync();

                _toastNotification.AddInfoToastMessage("Edited successfully.");

                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                _toastNotification.AddErrorToastMessage("Error occured, try again later.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            var promoCode = await _context.promo_codes.FindAsync(id);

            if(promoCode == null)
            {
                _toastNotification.AddErrorToastMessage("No found on this specified id.");
                return RedirectToAction("Index");
            }

            _context.promo_codes.Remove(promoCode);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
