namespace LotteryManagement.Application.ViewModels.Inputs
{
    public class InputBankCard
    {
        public string BankName { get; set; }
        public string BankBranch { get; set; }

        public string FullNameOwner { get; set; }

        public string BankAccountNumber { get; set; }

        public string UserId { get; set; }

        public string ConfirmBankAccountNumber { get; set; }

        public string TransactionPassword { get; set; }

    }
}
