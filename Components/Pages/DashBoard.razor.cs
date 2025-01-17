using BytesTracker.Model;
using BytesTracker.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;


namespace BytesTracker.Components.Pages
{
    public partial class DashBoard
    {

        [Inject]
        private Dto.User userDto { get; set; }

        [Inject]

        private Dto.Transaction transactionDto { get; set; }

        private String currencyCode;
        private String currencySymbol;

        int transactioncounter = 15;
        
        private List<Model.Transaction> userTransactions = new();


        private int Index = -1; 
        int dataSize = 4;
        double[] data = { 77, 25, 20, 5 };

        string[] labels = { "Total Debts", "Total Credit", "Clear Debts", "Remaing debts" };
        private string[] colors = { "#FF6384", "#36A2EB", "#4BC0C0", "#FF9F40" };



        public List<ChartSeries> Series = new List<ChartSeries>();
        public string[] XAxisLabels = Array.Empty<string>();





        private enum ChartView
        {
            TypeDistribution,
            StatusDistribution,
            TagDistribution
        }


        public void StripCurency()
        {

            var currencyCombined = userDto.Currency.Split('-');
            currencyCode = currencyCombined[0];
            currencySymbol = currencyCombined[1];
        }

        private async Task LoadChartData()
        {
            try
            {
                var userName = await localStorage.GetItemAsync<string>("username");

                int userId = await userService.Get_UserID(userName);

                

                decimal totalDebit = await transactioService.GetTotalDebit(userId);
                decimal totalCredit = await transactioService.GetTotalCredit(userId);
                decimal clearedDebts = await transactioService.GetTotalCleard(userId);


                decimal remainingDebts = Math.Max(0, totalDebit - clearedDebts); 

                data = new double[]
                {
                Convert.ToDouble(totalDebit),
                Convert.ToDouble(totalCredit),
                Convert.ToDouble(clearedDebts),
                Convert.ToDouble(remainingDebts)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading chart data: {ex.Message}");
            }
        }

        private string GetChartLegend()
        {
            var legend = new StringBuilder();
            for (int i = 0; i < labels.Length; i++)
            {
                legend.Append($"{labels[i]}: {data[i]:C2}");
                if (i < labels.Length - 1) legend.Append(" | ");
            }
            return legend.ToString();
        }


        private async Task ClearDebt(Transaction transaction) {

          await transactioService.UpdateTransactionStatus(transaction);

            var userName = await localStorage.GetItemAsync<string>("username");
            int userId = await userService.Get_UserID(userName);
            userTransactions = await transactioService.GetTransactions(userId);
            StateHasChanged();



        }


        protected async Task OnSortChange(ChangeEventArgs e)
        {
            try
            {
                var selectedValue = e.Value.ToString();

                if (string.IsNullOrEmpty(selectedValue) || selectedValue == "default")
                {
                    var topTrans = userTransactions.OrderByDescending(trans => trans.amount).Take(5).ToList();
                    var lowTrans = userTransactions.OrderBy(trans => trans.amount).Take(5).ToList();

                    Series = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Top 5 Transactions",
                    Data = topTrans.Select(trans => (double)trans.amount).ToArray()
                },
                new ChartSeries
                {
                    Name = "Lowest 5 Transactions",
                    Data = lowTrans.Select(trans => (double)trans.amount).ToArray()
                }
            };

                    XAxisLabels = topTrans.Select(trans => trans.source).Concat(lowTrans.Select(trans => trans.source)).ToArray();
                }
                else if (selectedValue == "High")
                {
                    var sortedTrans = userTransactions.OrderByDescending(trans => trans.amount).Take(5).ToList();

                    Series = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Top 5 Transactions (High to Low)",
                    Data = sortedTrans.Select(trans => (double)trans.amount).ToArray()
                }
            };

                    XAxisLabels = sortedTrans.Select(trans => trans.source).ToArray();
                }
                else if (selectedValue == "Low")
                {
                    var sortedTrans = userTransactions.OrderBy(trans => trans.amount).Take(5).ToList();

                    Series = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Top 5 Transactions (Low to High)",
                    Data = sortedTrans.Select(trans => (double)trans.amount).ToArray()
                }
            };

                    XAxisLabels = sortedTrans.Select(trans => trans.source).ToArray();
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating chart data: {ex.Message}");
            }
        }




        protected override async Task OnInitializedAsync()
        {
            try
            {

                StripCurency();
                var userName = await localStorage.GetItemAsync<string>("username");
                int userId = await userService.Get_UserID(userName);
                //userTags = await tagService.GetUserTags(userId);

                

                userTransactions = await transactioService.GetTransactions(userId);
                
                var topTrans = userTransactions.OrderByDescending(trans => trans.amount).Take(5).ToList();
                var lowTrans = userTransactions.OrderBy(trans => trans.amount).Take(5).ToList();

                Series = new List<ChartSeries> {

                new ChartSeries{

                    Name="top 5 Transaction",
                    Data = topTrans.Select(trans => (double)trans.amount).ToArray()
                    },

                new ChartSeries{

                    Name = "Lowest 5 Transaction",
                    Data = lowTrans.Select(trans => (double)trans.amount).ToArray()
                    }

                };

                XAxisLabels = topTrans.Select(trans => trans.source).Concat(lowTrans.Select(trans => trans.source)).ToArray();
                transactionDto = new Dto.Transaction
                {
                    Status = Helper.StatusType.Pendling.ToString(),
                    Type = Helper.SourceType.Debit.ToString()

                };

                await LoadChartData();
                StateHasChanged();


            }
            catch (Exception e)
            {

               

            }


        }


    }
}