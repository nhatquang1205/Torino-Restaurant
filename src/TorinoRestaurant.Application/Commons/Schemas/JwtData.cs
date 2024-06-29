namespace TorinoRestaurant.Application.Commons.Schemas
{
	public class JwtData
	{
		public long UserId { set; get; }
        public required string Username { set; get; }
        public required string Aud { set; get; }
        public required string Iss { set; get; }
        public required string Secret { set; get; }
    }
}

