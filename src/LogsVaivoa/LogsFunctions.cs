using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using LogsVaivoa;
using LogsVaivoa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Hosting;
using Newtonsoft.Json;

[assembly: WebJobsStartup(typeof(Startup))]
namespace LogsVaivoa
{
    

    public class LogsFunction
    {
        private readonly LogService _logService;
        private readonly ApplicationInsightService _appService;
        public LogsFunction(LogService logService, ApplicationInsightService appService)
        {
            _logService = logService;
            _appService = appService;
        }

        [FunctionName("LogsFunction")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Log), Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Log), Description = "Created")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(List<ValidationFailure>), Description = "Fail Validation")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequest req)
        {
           // _log.Information("C# HTTP trigger function processed a request.");

           await _appService.SendMetricToElastic();
            

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Log>(requestBody);

            var (status, result) = await _logService.InsertLog(data);

            if (status) return new CreatedResult("", result);

            return new BadRequestObjectResult(result);
        }
    }

    public class Log
    {
        public string Nome { get; set; }
        public string Mensagem { get; set; }
        public string Detalhe { get; set; }


        public List<ValidationFailure> GetErrors() => new LogModelValidation().Validate(this).Errors;

    }

    public class LogModelValidation : AbstractValidator<Log>
    {
        public LogModelValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(50).WithMessage("O campo {PropertyName} deve ter no maximo 50 caracteres");
            
            RuleFor(c => c.Mensagem)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(250).WithMessage("O campo {PropertyName} deve ter no maximo 50 caracteres");
            
            RuleFor(c => c.Detalhe)
                .MaximumLength(1000).WithMessage("O campo {PropertyName} deve ter no maximo 50 caracteres");

        }
    }

}

