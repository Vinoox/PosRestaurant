using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Validation
{
    public static class ValidationExtension
    {
        public static IRuleBuilderOptions<T, string> RoleMustExist<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            RoleManager<IdentityRole> roleManager)
        {
            return ruleBuilder.MustAsync(async (roleName, cancellationToken) =>
            {
                if (string.IsNullOrEmpty(roleName))
                {
                    return true;
                }

                return await roleManager.RoleExistsAsync(roleName);
            })
            .WithMessage("Podana rola '{PropertyValue}' nie istnieje w systemie.");
        }
    }
}
