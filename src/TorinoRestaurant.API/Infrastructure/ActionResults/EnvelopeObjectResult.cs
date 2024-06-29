using Microsoft.AspNetCore.Mvc;

namespace TorinoRestaurant.API.Infrastructure.ActionResults
{
    public class EnvelopeObjectResult : ObjectResult
    {
        public EnvelopeObjectResult(Envelope envelope)
            : base(envelope)
        {
            StatusCode = envelope.Status;
        }
    }
}