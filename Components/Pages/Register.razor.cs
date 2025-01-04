namespace BytesTracker.Components.Pages
{
    public partial class Register
    {
        private Dto.User userDto = new();
       
        private bool isError = false;
        private bool isSuccess = false;
        private string errorMessage = string.Empty;

        private async Task HandleRegister()
        {
            try
            {
                isError = false;
                isSuccess = false;

                if (string.IsNullOrWhiteSpace(userDto.UserName) ||
                    string.IsNullOrWhiteSpace(userDto.Password))

                {
                    isError = true;
                    errorMessage = "All fields are required";
                    return;
                }

                
                bool isRegistered = await UserService.IsUserRegistered(userDto.UserName);
                if (isRegistered)
                {

                    isError = true;
                    errorMessage = "A user with this Name is already registered.";
                    return;
                }

                
                var user = new Model.Users
                {
                    UserName = userDto.UserName,
                    Password = userDto.Password
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