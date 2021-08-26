using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Common
{
    // form bt
    //public class ValidationError
    //{
    //    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //    public string Property { get; }
    //    public string Message { get; }

    //    public ValidationError(string field, string message)
    //    {
    //        Property = field != string.Empty ? field : null;
    //        Message = message;
    //    }
    //}

    public class ValidationResultModel
    {
        private string status = ResultCode.ORROR;
        private int result = 0;
        public string Message { get; }

        public string Success { get { return status; } set { status = value; } }

        public int Result { get { return result; } set { result = value; } }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            //Message = modelState.Keys
            //        .Select(key => modelState[key].Errors.Select(x => x.ErrorMessage)).ToArray()[0].FirstOrDefault()[0].e;
            Message = modelState.Keys.Select(x => modelState[x].Errors)
                           .FirstOrDefault().ToArray()[0].ErrorMessage;
        }
    }

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }

    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    // form data

    public static class ValidationForFormData
    {
        public static string validObject(Object n)
        {
            var context = new ValidationContext(n, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(n, context, validationResults, true);

            if (!isValid)
            {
                return validationResults[0].ErrorMessage;
            }
            return null;
        }
    }
}
