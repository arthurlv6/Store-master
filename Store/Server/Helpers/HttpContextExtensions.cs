using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Server
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationParameterInResponse(this HttpContext httpContext,
            double pagesQuantity)
        {
            httpContext.Response.Headers.Add("pagesQuantity", pagesQuantity.ToString());
        }
    }
}
