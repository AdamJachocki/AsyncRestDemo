namespace Common.Models
{
    public enum OperationStatus
    {
        NotStarted,
        InProgress,
        Finished
    }
    public class WeatherDatabaseItem
    {
        public string Id => RequestId.ToString();
        public Guid RequestId { get; set; }
        public OperationStatus Status { get; set; }
        public WeatherForecast Data { get; set; }
    }
}