namespace Bets.Services
{
    public class BetModel
    {
        public string MoreSide { get; }
        public string LessSide { get; }
        public decimal Amount { get; }
        public string Category { get; }
        public string Date { get; }
        public string Val1 { get; }
        public string Val2 { get; }
        public int Side { get; }

        public BetModel(string moreSide, string lessSide, decimal amount, string category, string date, string val1, string val2, int side)
        {
            MoreSide = moreSide;
            LessSide = lessSide;
            Amount = amount;
            Category = category;
            Date = date;
            Val1 = val1;
            Val2 = val2;
            Side = side;
        }

        protected bool Equals(BetModel other)
        {
            return string.Equals(MoreSide, other.MoreSide) && string.Equals(LessSide, other.LessSide) && Amount == other.Amount && string.Equals(Category, other.Category) && string.Equals(Date, other.Date) && string.Equals(Val1, other.Val1) && string.Equals(Val2, other.Val2) && Side == other.Side;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BetModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (MoreSide != null ? MoreSide.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LessSide != null ? LessSide.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Date != null ? Date.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Val1 != null ? Val1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Val2 != null ? Val2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Side;
                return hashCode;
            }
        }
    }
}
