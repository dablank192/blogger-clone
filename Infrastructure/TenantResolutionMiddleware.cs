using System;
using Microsoft.EntityFrameworkCore;

namespace blogger_clone.Infrastructure;

public class TenantResolutionMiddleware(
    RequestDelegate Next,
    IConfiguration Config
)

{
    public async Task InvokeAsync(HttpContext httpContext, AppDbContext dbContext)
    {
        var host = httpContext.Request.Host.Value!.ToLower();

        var baseDomain = Config["AppConfig:BaseDomain"];

        string subDomain = string.Empty;

        if(
            !string.IsNullOrEmpty(baseDomain)
            && host.EndsWith(baseDomain)
            && host != baseDomain
        )
        {
            subDomain = host.Replace($".{baseDomain}", "");
        }

        if(!string.IsNullOrEmpty(subDomain))
        {
            var blogId = await dbContext.Blog.Where(t => t.SubDomain == subDomain)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();

            if(blogId != Guid.Empty)
            {
                httpContext.Items["BlogId"] = blogId;
            }
        }

        await Next(httpContext);
    }
}
