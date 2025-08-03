using ExchangeRateApiMockServer.Models;
using ExchangeRateApiMockServer.Shared.Env;
using ExchangeRateApiMockServer.Shared.Utils;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace ExchangeRateApiMockServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class ExchangeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ExchangeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("api1/convert")]
        public IActionResult Api1Convert([FromBody] Api1Request request)
        {
            var validApiKey = ReadConfigValueMock.GetConfigValue(_configuration, keyConfig: EnvironmentVariablesMock.APIKEY);

            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (apiKey != validApiKey)
                return Unauthorized(new { code = HttpStatusCode.Unauthorized, message = "Invalid API Key" });

            var rateApi = Api1MockRates.GetRate(request.from, request.to);

            return Ok(new { rate = rateApi });
        }

        [HttpPost("api2/convert")]
        public async Task<IActionResult> Api2Convert()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var unauthorizeResponse = new XElement("XML", new XElement("Code", 401),
                    new XElement("Message", "Invalid Basic Auth"));
            if (!IsValidBasicAuth(authHeader))
                return Unauthorized(Content(unauthorizeResponse.ToString(), "application/xml").Content);
            
            using var reader = new StreamReader(Request.Body);
            var xmlString = await reader.ReadToEndAsync();
            var xml = XElement.Parse(xmlString);

            var from = xml.Element("From")?.Value;
            var to = xml.Element("To")?.Value;
            var amountStr = xml.Element("Amount")?.Value;

            if (!decimal.TryParse(amountStr, out var amount))
                return BadRequest("Invalid amount");

            var rate = Api2MockRates.GetRate(from!, to!);
            var totalAmount = amount * rate;
            var roundedValue = Math.Round(totalAmount, 2);

            
            var response = new XElement("XML", new XElement("Result", roundedValue));
            return Content(response.ToString(), "application/xml");
        }

        [HttpPost("api3/convert")]
        public IActionResult Api3Convert([FromBody] Api3Request request)
        {
            var token = ReadConfigValueMock.GetConfigValue(_configuration, keyConfig: EnvironmentVariablesMock.APITOKEN);

            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Bearer ") || authHeader[7..] != token)
                return Unauthorized(new { code = HttpStatusCode.Unauthorized, message = "Invalid Bearer Token" });

            var from = request.exchange.sourceCurrency;
            var to = request.exchange.targetCurrency;
            var amount = request.exchange.quantity;

            var rate = Api3MockRates.GetRate(from, to);
            var totalAmount = Math.Round(amount * rate, 2);

            return Ok(new
            {
                StatusCode = 200,
                message = "success",
                data = new { total = totalAmount }
            });
        }

        private bool IsValidBasicAuth(string authHeader)
        {
            var apiUser = ReadConfigValueMock.GetConfigValue(_configuration, keyConfig: EnvironmentVariablesMock.APIUSER);
            var apiPassword = ReadConfigValueMock.GetConfigValue(_configuration, keyConfig: EnvironmentVariablesMock.APIPASS);

            if (!authHeader.StartsWith("Basic ")) return false;

            var encoded = authHeader[6..];
            var decodedBytes = Convert.FromBase64String(encoded);
            var decoded = Encoding.UTF8.GetString(decodedBytes);

            var parts = decoded.Split(':');
            return parts.Length == 2 &&
                   parts[0] == apiUser &&
                   parts[1] == apiPassword;
        }
    }
}
