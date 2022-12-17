using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FamilyBudget.Tests.Common;

public static class ControllerHelper
{
    public static void AddValidationErrors(this ModelStateDictionary modelState, object model)
    {
        var context = new ValidationContext(model, null, null);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(model, context, results, true);
        foreach (var result in results)
        {
            var name = result.MemberNames.First();
            modelState.AddModelError(name, result.ErrorMessage);
        }
    }
}