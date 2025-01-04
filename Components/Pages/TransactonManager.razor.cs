using Microsoft.AspNetCore.Components;

namespace BytesTracker.Components.Pages
{
    public partial class TransactonManager
    {

        [Inject]
        private Dto.User userDto { get; set; }
        [Inject]
        private Dto.Transaction transactionDto { get; set; }



        private string statusListner;
        int tagcounter = 0;
        int transactioncounter = 0;

        private bool showTransPop = false;

        

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


        private Helper.StatusType selectedStatus = Helper.StatusType.Pendling;
        private bool onStatusChanage() {

            return true;
        }




        private void StripCurency() {

            var currencyCombined = userDto.Currency.Split('-');
            currencyCode = currencyCombined[0];
            currencySymbol = currencyCombined[1];
        }


        private void ShowtransactionManagerPopUp()
        {
            showTransPop = true;
        }
        private async void AddTransation() {
            var userName = await localStorage.GetItemAsync<string>("username");
            var userId = await userService.Get_UserID(userName);


            var transaction = new Model.Transaction()
            {
                user_id = userId,
                source = transactionDto.Sources,
                amount = transactionDto.Value,
                due_at = (DateTime)transactionDto.DueDate,
                note = transactionDto.Note,
                status = transactionDto.Status,
                type = transactionDto.Type,
                tagid = transactionDto.Tag,
            };

            await transactioService.AddTransaction(transaction);
            await Task.Delay(1500);
            userTransactions = await transactioService.GetTransactions(userId);
            StateHasChanged();


        }

        private async void ClosetransactionManagerPopUp()
        {
            showTransPop = false;

        }

        private void CloseErrorPopUp()
        {
            isError = false;
            errorMessage = string.Empty;
            StateHasChanged();

        }

        

        private void CloseSuccessPopUp()
        {
            isSuccess = false;
            StateHasChanged();

        }

        private async Task getAllTags()
        {
            var UserName = await localStorage.GetItemAsync<string>("username");
            int userId = await userService.GetUserIdByUsername(UserName);
            userTags = await tagService.GetUserTags(userId);

        }

        private async Task HandleDeleteTag(int tagID)
        {

            var UserName = await localStorage.GetItemAsync<string>("username");

            int userId = await userService.GetUserIdByUsername(UserName);

            await tagService.DeleteTag(tagID, userId);

            userTags = await tagService.GetUserTags(userId);


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
                    Type = Helper.SourceType.Debit.ToString(),
                    Tag = 0

                };

            }
            catch (Exception e)
            {

                isError = true;
                errorMessage = "Initialization failed: " + e.Message;

            }


        }



    }
}