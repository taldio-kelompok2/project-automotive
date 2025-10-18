using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Transaction
{
    public class TransactionViewModel
    {
        // private readonly string ImagePath = "Icons/Transactions/";
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        // public TransactionCategory Category = TransactionCategory.BCA;
        // public string Image { get; set; } = null!;
        public string? Image { get; set; }

        public TransactionViewModel() { }

        // public TransactionViewModel(TransactionCategory category)
        // {
        //     Category = category;
        //     Image = ImagePath + category.ToString().ToLower() + ".svg";
        // }

        // public static List<TransactionViewModel> InitDummyData()
        // {
        //     var data = new List<TransactionViewModel>();

        //     foreach (TransactionCategory category in Enum.GetValues<TransactionCategory>())
        //     {
        //         data.Add(new TransactionViewModel(category));
        //     }

        //     return data;
        // }
    }
}