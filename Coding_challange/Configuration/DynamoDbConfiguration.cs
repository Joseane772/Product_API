namespace Coding_challange.Configuration
{
    public class DynamoDbConfiguration
    {
        public bool UseLocalStack { get; set; }
        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string Region { get; set; }
        public string ServiceURL { get; set; }
        
    }
}