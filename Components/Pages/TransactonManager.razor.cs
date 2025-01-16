using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using MudBlazor;
using System.Transactions;

namespace BytesTracker.Components.Pages
{
    public partial class TransactonManager
    {

        [Inject]
        private Dto.User userDto { get; set; }
        [Inject]
        private Dto.Transaction transactionDto { get; set; }
        [Inject]
        private Dto.Tags tagDto { get; set; }
        [Inject]
        private Dto.SortFormDTO sortFormDto { get; set; }




        decimal totalCredit;
        decimal totalDebit;
        decimal totalBalance;

        private string statusListner;
        int tagcounter = 0;
        int transactioncounter = 0;

        private bool showTransPop = false;
        private bool showTransactionSuccessMessage = false;
        private bool showTransactionFailureMessage = false;
        private bool isEditMode = false;

        private Model.Transaction currentTransaction = new();




        private Dto.Tags tagModel = new();



        private bool isError = false;
        private bool isSuccess = false;
        private string errorMessage = string.Empty;

        private List<Model.Tags> userTags = new();
        private List<string> tagNames = new();
        private List<Model.Transaction> userTransactions = new();

        private Helper.SourceType selectType;

        private String currencyCode;
        private String currencySymbol;


        private string selectedSortOption = "Sort by";
        private bool showTags = false;







        private Helper.StatusType selectedStatus = Helper.StatusType.Pendling;

        public List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "United States", Data = new double[] { 40, 20, 25, 27, 46, 60, 48, 80, 15 } },
        new ChartSeries() { Name = "Germany", Data = new double[] { 19, 24, 35, 13, 28, 15, 13, 16, 31 } },
        new ChartSeries() { Name = "Sweden", Data = new double[] { 8, 6, 11, 13, 4, 16, 10, 16, 18 } },
    };






        private async void onSortChange(ChangeEventArgs e) {

            selectedSortOption = e.Value.ToString();
            sortFormDto.SortBy = selectedSortOption; 

            if (selectedSortOption == "Debit" || selectedSortOption == "Credit" || selectedSortOption == "Date" || selectedSortOption == "Amount")
            {
                sortFormDto.showHighLow = true;
            }
            else {
                sortFormDto.showHighLow = false;

            }


            if (selectedSortOption == "Tag")
            {
                sortFormDto.showTags = true;


            }
            else {

                sortFormDto.showTags = false;

            }

            StateHasChanged();

        }

        


        private bool onStatusChanage() {

            return true;
        }




        public void StripCurency() {

            var currencyCombined = userDto.Currency.Split('-');
            currencyCode = currencyCombined[0];
            currencySymbol = currencyCombined[1];
        }


        private void ShowtransactionManagerPopUp()
        {
            showTransPop = true;
        }
        private async Task ShowTransEditMode(Model.Transaction transaction)
        {
            transactionDto = new Dto.Transaction
            {
                Sources = transaction.source,
                Value = transaction.amount,
                DueDate = transaction.due_at,
                Status = transaction.status,
                Type = transaction.type,
                Note = transaction.note
            };

            tagDto.TagName = transaction.tagname;
            currentTransaction = transaction;

            showTransPop = true;
            isEditMode = true;
            StateHasChanged();
        }

        


        private async void ClosetransactionManagerPopUp()
        {
            showTransPop = false;
            showTransactionFailureMessage = false;
            showTransactionSuccessMessage = false;
            transactionDto = new Dto.Transaction
            {
                Status = Helper.StatusType.Pendling.ToString(),
                Type = Helper.SourceType.Debit.ToString()
            };
            tagDto = new Dto.Tags();
            currentTransaction = new Model.Transaction();
            isEditMode = false;
            StateHasChanged();
        }
        private async void AddTransation()
        {
            try
            {
                var userName = await localStorage.GetItemAsync<string>("username");
                var userId = await userService.Get_UserID(userName);

                if (transactionDto.Type.ToLower() == "debit")
                {
                    var currentTotalCredit = await transactioService.GetTotalCredit(userId);
                    var currentTotalDebit = await transactioService.GetTotalDebit(userId);
                    var currentBalance = currentTotalCredit - currentTotalDebit;

                    if (transactionDto.Value <= 0)
                    {
                        showTransactionSuccessMessage = false;
                        showTransactionFailureMessage = true;
                        errorMessage = "Transaction value must be greater than zero.";
                        StateHasChanged();
                        return;
                    }

                    var newBalance = currentBalance - transactionDto.Value;

                    if (newBalance < 0)
                    {
                        showTransactionSuccessMessage = false;
                        showTransactionFailureMessage = true;
                        errorMessage = "Transaction can't be added. Insufficient total balance.";
                        StateHasChanged();
                        return;
                    }
                }

                var transaction = new Model.Transaction()
                {
                    user_id = userId,
                    source = transactionDto.Sources,
                    amount = transactionDto.Value,
                    due_at = (DateTime?)transactionDto.DueDate,
                    note = transactionDto.Note,
                    status = transactionDto.Status,
                    type = transactionDto.Type,
                    tagname = tagDto.TagName
                };

                await transactioService.AddTransaction(transaction);

                await Task.Delay(1500);
                userTransactions = await transactioService.GetTransactions(userId);
                totalCredit = await transactioService.GetTotalCredit(userId);
                totalDebit = await transactioService.GetTotalDebit(userId);
                totalBalance = totalCredit - totalDebit;

                showTransactionSuccessMessage = true;
                showTransactionFailureMessage = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                showTransactionSuccessMessage = false;
                showTransactionFailureMessage = true;
                errorMessage = $"Failed to add transaction: {ex.Message}";
                StateHasChanged();
            }
        }


        private async Task UpdateTransaction()
        {
            try
            {
                var userName = await localStorage.GetItemAsync<string>("username");
                var userId = await userService.Get_UserID(userName);
                DateTime? dueAt;


                if (transactionDto.Type.ToLower() == "debit")
                {
                    var currentTotalCredit = await transactioService.GetTotalCredit(userId);
                    var currentTotalDebit = await transactioService.GetTotalDebit(userId);
                    var currentBalance = currentTotalCredit - currentTotalDebit;

                    if (isEditMode && currentTransaction.type.ToLower() == "debit")
                    {
                        currentBalance += currentTransaction.amount;
                    }

                    var newBalance = currentBalance - transactionDto.Value;

                    if (newBalance < 0)
                    {
                        showTransactionSuccessMessage = false;
                        showTransactionFailureMessage = true;
                        errorMessage = "Transaction can't be updated. Insufficient total balance";
                        StateHasChanged();
                        return;
                    }
                }


                if (transactionDto.Status == Helper.StatusType.Pendling.ToString())
                {
                    dueAt = (DateTime)transactionDto.DueDate;
                }
                else
                {
                    dueAt = null;
                }

                var updatedTransaction = new Model.Transaction
                {
                    id = currentTransaction.id,
                    user_id = userId,
                    source = transactionDto.Sources,
                    amount = transactionDto.Value,
                    due_at = dueAt,
                    note = transactionDto.Note,
                    status = transactionDto.Status,
                    type = transactionDto.Type,
                    tagname = tagDto.TagName
                };

                await transactioService.UpdateTransaction(updatedTransaction, currentTransaction.id);

                userTransactions = await transactioService.GetTransactions(userId);
                totalCredit = await transactioService.GetTotalCredit(userId);
                totalDebit = await transactioService.GetTotalDebit(userId);
                totalBalance = totalCredit - totalDebit;

                showTransactionSuccessMessage = true;
                await Task.Delay(1500);
                ClosetransactionManagerPopUp();
            }
            catch (Exception ex)
            {
                showTransactionFailureMessage = true;
                errorMessage = $"Failed to update transaction: {ex.Message}";
            }
        }


        private async void AddOrUpdateTransaction()
        {
            if (isEditMode)
            {
                await UpdateTransaction();
            }
            else
            {
                AddTransation();
            }
        }

        private void CloseErrorPopUp()
        {
            isError = false;
            errorMessage = string.Empty;
            StateHasChanged();

        }

        private void CloseSuccessPopUp()
        {
            showTransactionSuccessMessage = false;
            isSuccess = false;
            StateHasChanged();

        }

        private async Task getAllTags()
        {
            var UserName = await localStorage.GetItemAsync<string>("username");
            int userId = await userService.GetUserIdByUsername(UserName);
            userTags = await tagService.GetUserTags(userId);

        }


        private async Task TransanctionSort() {

            var userName = await localStorage.GetItemAsync<string>("username");
            var userId = await userService.GetUserIdByUsername(userName);
            userTransactions = await transactioService.GetSortedTransaction(userId, sortFormDto);
            StateHasChanged();
        }


        private async Task HandleDeleteTag(int tagID)
        {

            var UserName = await localStorage.GetItemAsync<string>("username");

            int userId = await userService.GetUserIdByUsername(UserName);

            await tagService.DeleteTag(tagID, userId);

            userTags = await tagService.GetUserTags(userId);


            StateHasChanged();
        }

        private void HandleStatus() {
            if (transactionDto.Status != Helper.StatusType.Pendling.ToString())
            {
                transactionDto.DueDate = null;
            }
            StateHasChanged();


        }



        private async Task HandleSubmit()
        {

            try
            {

                isError = false;
                isSuccess = false;
                if (string.IsNullOrWhiteSpace(tagModel.TagName) ||
                    string.IsNullOrWhiteSpace(tagModel.TagDescription))
                {
                    isError = true;
                    errorMessage = "All fields are required";
                    return;
                }
                var UserName = await localStorage.GetItemAsync<string>("username");
                int userID = await userService.Get_UserID(UserName);

                var tags = new Model.Tags
                {
                    user_id = userID,
                    TagName = tagModel.TagName,
                    TagDescription = tagModel.TagDescription

                };

                await tagService.AddTag(tags);
                isSuccess = true;
                await Task.Delay(1500);
                userTags = await tagService.GetUserTags(userID);
                StateHasChanged();


            }
            catch (Exception e)
            {
                isError = true;
                errorMessage = "Adding Tag Failed: " + e.Message;
            }

        }



        protected override async Task OnInitializedAsync()
        {
            try
            {

                StripCurency();
                var userName = await localStorage.GetItemAsync<string>("username");
                int userId = await userService.Get_UserID(userName);
                userTags = await tagService.GetUserTags(userId);
                userTransactions = await transactioService.GetTransactions(userId);
                transactionDto = new Dto.Transaction
                {
                    Status = Helper.StatusType.Pendling.ToString(),
                    Type = Helper.SourceType.Debit.ToString()

                };

                totalCredit = await transactioService.GetTotalCredit(userId);
                totalDebit = await transactioService.GetTotalDebit(userId);
                totalBalance = totalCredit - totalDebit;

            }
            catch (Exception e)
            {

                isError = true;
                errorMessage = "Initialization failed: " + e.Message;

            }


        }



    }
}