﻿using Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttributeForDoctor : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
            var userManager = validationContext.GetService<UserManager<ApplicationUser>>();

            // Get the current user's ID unless it's null
            var currentUser = httpContextAccessor.HttpContext?.User;
            var userId = userManager.GetUserId(currentUser);

            // Check if the user has the "doctor" role
            bool isDoctor = false;
            if (!string.IsNullOrEmpty(userId))
            {
                var user = userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                isDoctor = user != null && userManager.IsInRoleAsync(user, "doctor").GetAwaiter().GetResult();
            }

            // If the user is a doctor, the property is required
            if (isDoctor)
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }

}
