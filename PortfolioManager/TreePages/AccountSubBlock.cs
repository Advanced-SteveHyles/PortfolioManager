namespace PortfolioManager.ViewModels
{
    public class AccountSubBlock
    {
        public AccountSubBlock(string funds, int i)
        {

            Title = funds;
            Rating = i;
        }

        public string Title { get; set; }
        public int Rating { get; set; }
    }
}