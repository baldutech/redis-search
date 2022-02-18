using Redis.Search.Shared.Domain.Enums;
using System;

namespace Redis.Search.Shared.Domain.Funds
{
    public class Fund
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Class { get; set; }
        public string? Start { get; set; }
        public string? Manager { get; set; }
        public string? Administrator { get; set; }

        public DateTime? StartFormatted =>
            Start == null
            ? DateTime.Today
            : DateTime.Parse(Start);

        public int? FundTypeFormatted => 1;
            //Type == null
            //? null
            //: FundType.FromValue(Convert.ToInt32(Type))?.Id;
    }
}
