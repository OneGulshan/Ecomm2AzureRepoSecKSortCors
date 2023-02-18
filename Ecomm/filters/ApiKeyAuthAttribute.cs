using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecomm.filters
{//Now we can use this filter on any Controller
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)] // here targated our class and methods so we req this validate by validOn method simple
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter // HttpGet & Roles also inherited by this class, IAsyncActionFilter interface for making secret key
    {
        private const string ApiKeyHeaderName = "ApiKey"; // our api key value is ApiKey here defined, req match ApiKey header here from Postman
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) // OnActionExecutionAsync its means jab ham ApiKeyAuthAttributes as a filter attribute use karenge tab ye method call ho jaega, ye current http req get karta hai using Context
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName,out var potentialKey))//jo req hamare pass aati hai Potential(Postman ke header se) key, HttpContext means current session, jo bhi Postman header's req potentialKey hai vo yahan not null chk ki hai
            {
                context.Result = new UnauthorizedResult(); // agar ApiKey match nahi karti hai to ham yaha httpcontext me UnauthorizedResult pass kar denge
                return;
            }
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>(); // for getting ApiKey from appsettings using IConfiguration
            var apikey = configuration.GetValue<string>(key: "ApiKey"); // in API json formate in key value pair key is ApiKey here ok
            if (!apikey.Equals(potentialKey)) // In case data sent directly from url or not matched
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next(); // if gotted result then pass on to next
        }
    }
}
//context vo hota hai jisme req ki jaati hai Postman ke header se pass karva ke agr vo mtc nhi karti to UnauthorizedResult() hamare ret kar diya jata hai