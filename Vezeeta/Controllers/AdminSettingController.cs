﻿using Core.Domain;
using Core.DTO;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.RegularExpressions;

namespace Vezeeta.Controllers
{
    [Route("api/Admin/DiscountCodeCoupon")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminSettingController : ControllerBase
    {
        private readonly IDiscountCodeCouponServices _discountCodeCouponServices;
        private readonly IApplicationUserService _adminServices;

        public AdminSettingController(IDiscountCodeCouponServices DiscountCodeCouponServices,
            IApplicationUserService AdminServices) 
        {
            _discountCodeCouponServices = DiscountCodeCouponServices;
            _adminServices = AdminServices;
        }

        #region DiscountCodeCoupon APIs
        [HttpPut]
        public IActionResult UpdateDiscountCodeCoupon([FromBody]DiscountCodeCoupon DiscountCodeCoupon)
        {
            if (DiscountCodeCoupon == null)
            {
                ModelState.AddModelError("DiscountCodeCoupon", "The DiscountCodeCoupon is required.");
            }
           else if (DiscountCodeCoupon.Id == default)
            {
                ModelState.AddModelError("Id", "The Id is required.");
            }

            else if (DiscountCodeCoupon.Id <0)
            {
                ModelState.AddModelError("Id", "The Id is Invalid. Must be greater than 0.");
            }

            if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                };

                return _discountCodeCouponServices.Update(DiscountCodeCoupon); 

        }

        [HttpPost]
        public async Task<IActionResult> AddDiscountCodeCoupon([FromBody]DiscountCodeCoupon DiscountCodeCoupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            };

            return await _discountCodeCouponServices.Add(DiscountCodeCoupon);

        }

        [HttpDelete]
        public IActionResult DeleteDiscountCodeCoupon([FromForm]int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id", "The Id is Invalid. Must be greater than 0.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            };

            return _discountCodeCouponServices.Delete(id);
        }

        [HttpPatch]
        [Route("Deactivate")]
        public IActionResult DeactivateDiscountCodeCoupon([FromForm] int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id", "The Id is Invalid. Must be greater than 0.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            };

            return _discountCodeCouponServices.Deactivate(id);

        }
        #endregion
    }
}
