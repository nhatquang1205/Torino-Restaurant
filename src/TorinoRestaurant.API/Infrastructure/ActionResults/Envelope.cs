using System.Diagnostics;
using System.Net;

namespace TorinoRestaurant.API.Infrastructure.ActionResults
{
    public class Envelope
    {
        public int Status { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
        public string? TraceId { get; set; }

        protected Envelope(int status, string? errorMessage, DateTime timestamp, string? traceId)
        {
            Status = status;
            ErrorMessage = errorMessage;
            Timestamp = timestamp;
            TraceId = traceId;
        }

        protected Envelope()
        {

        }

        public static Envelope Create(string error, HttpStatusCode statusCode)
        {
            return new Envelope((int)statusCode, error, DateTime.UtcNow, Activity.Current?.Id);
        }

        public EnvelopeObjectResult ToActionResult()
        {
            return new EnvelopeObjectResult(this);
        }
    }

    public class CreatedResultEnvelope(string id)
    {   
        public string Id { get; set; } = id;
    }
}