namespace Redis.Search.Shared.Domain.Funds
{
    public class Fund
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? Start { get; set; }
        public string? Manager { get; set; }
        public string? FundType { get; set; }
        public string? Administrator { get; set; }

        public string FundTypeFormatted =>
            string.IsNullOrEmpty(FundType)
                ? Enums.FundType.Opened.Name
                : Enums.Enumeration.FromValue<Enums.FundType>(FundType)?.Name ?? Enums.FundType.Opened.Name;
    }
}
