namespace Redis.Search.Shared.Domain.Enums
{
    public class FundType : Enumeration
    {
        public static readonly FundType Xp = new FundType(1, "Aberto");
        public static readonly FundType Rico = new FundType(2, "Fechado");

        public FundType(int id, string name) : base(id, name)
        {
        }
    }
}
