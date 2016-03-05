namespace PortfolioManager
{
    public class TabViewModel
    {
        private AccountTabViewModel _accountTabViewModel;

        public AccountTabViewModel AccountTabViewModel => _accountTabViewModel ?? (_accountTabViewModel = new AccountTabViewModel());
    }

    public class AccountTabViewModel
    {
        public AccountTabViewModel()
        {
            int i = 1;
        }
    }
}