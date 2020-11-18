using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.CoreLogging
{
    public class TrackPerformanceFilter : IActionFilter
    {
        private PerfTracker _tracker;
        private string _product, _layer;

        public TrackPerformanceFilter(string product, string layer)
        {
            _product = product;
            _layer = layer;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var activity = $"{request.Path}-{request.Method}";

            var dict = new Dictionary<string, object>();
            foreach (var key in context.RouteData.Values?.Keys)
                dict.Add($"RouteData-{key}", (string)context.RouteData.Values[key]);

            var details = WebHelper.GetWebLogDetail(_product, _layer, activity, context.HttpContext, dict);
            _tracker = new PerfTracker(details); //initialize

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_tracker != null)
            {
                _tracker.Stop();
            }
        }
    }
}
