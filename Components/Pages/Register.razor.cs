namespace BytesTracker.Components.Pages
{
    public partial class Register
    {
        private Dto.Register registerModel = new();
        private bool isError = false;
        private bool isSuccess = false;
        private string errorMessage = string.Empty;

        private async Task HandleRegister()
        {
            try
            {
                isError = false;
                isSuccess = false;

                if (string.IsNullOrWhiteSpace(registerModel.UserName) ||
                    string.IsNullOrWhiteSpace(registerModel.Password))

                {
                    isError = true;
                    errorMessage = "All fields are required";
                    return;
                }

                // Check if the user is already registered
                bool isRegistered = await UserService.IsUserRegistered(registerModel.UserName);
                if (isRegistered)
                {
                    isError = true;
                    errorMessage = "A user with this Name is already registered.";
                    return;
                }

                // Proceed with registration
                var user = new Model.Users
                {
                    UserName = registerModel.UserName,
                    Password = registerModel.Password
                };

                await UserService.Create_User(user);
                isSuccess = true;
                await Task.Delay(1500);
            }
            catch (Exception ex)
            {
                isError = true;
                errorMessage = "Login failed: " + ex.Message;
            }
        }
    }
}