namespace TorinoRestaurant.Application.Commons
{
    public class MinioSettings
    {
        public string AccessKey { get; set;} = string.Empty;
        public string Endpoint { get; set;} = string.Empty;
        public string SecretKey { get; set;} = string.Empty;
        public string BucketName { get; set;} = string.Empty;
        public string Protocol { get; set;} = string.Empty;
    }
}