namespace WebAppReact.DomainModel.Queries.EstimatedWaitingTime
{
    public class EstimatedWaitingTimeResponse
    {
        public int NumberOfUnansweredCalls { get; set; }

        public int EstimatedCallDurationInMinutes { get; set; }

        public int EstimatedWaitingTimeInMinutes { get; set; }
    }
}