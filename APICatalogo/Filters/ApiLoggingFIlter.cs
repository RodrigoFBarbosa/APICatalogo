using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFIlter : IActionFilter
{
    private readonly ILogger<ApiLoggingFIlter> _logger;

    public ApiLoggingFIlter(ILogger<ApiLoggingFIlter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        //execute após a Action
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("##################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"StatusCOde : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("##################################");

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //executa antes da Action
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("##################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("##################################");
    }
}
