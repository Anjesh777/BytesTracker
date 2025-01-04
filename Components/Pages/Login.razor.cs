namespace BytesTracker.Components.Pages
{
    public partial class Login
    {

        private Dto.Login loginModel = new();
        private bool isError = false;
        private bool isSuccess = false;
        private string errorMessage = string.Empty;

        private async Task HandleLogin()
        {
            isError = false;
            isSuccess = false;
            if (string.IsNullOrWhiteSpace(loginModel.UserName) ||
                   string.IsNullOrWhiteSpace(loginModel.Password) ||
                   string.IsNullOrWhiteSpace(loginModel.Currency))
            {
                isError = true;
                errorMessage = "All fields are required";
                return;
            }
            bool isValid = await UserService.Login_User(loginModel.UserName, loginModel.Password);
            if (isValid)
            {
                await localStrorage.SetItemAsStringAsync("username", loginModel.UserName);
                AuthState.IsAuthenticated = true;
                Nav.NavigateTo("/transaction");
            }
            else
            {
                isError = true;
                errorMessage = "Invalid username or password";
            }


        }


    }
}