using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Text.Json;

namespace BytesTracker.Components.Pages
{
    public partial class Login
    {

        [Inject]
        private Dto.User userDto { get; set; }

        private bool isError = false;
        private bool isSuccess = false;
        private string errorMessage = string.Empty;


        

        private async Task HandleLogin()
        {
            isError = false;
            isSuccess = false;


            if (string.IsNullOrWhiteSpace(userDto.UserName) ||
                   string.IsNullOrWhiteSpace(userDto.Password) ||
                   string.IsNullOrWhiteSpace(userDto.Currency))
            {
                isError = true;
                errorMessage = "All fields are required";
                return;
            }

          


            bool isValid = await UserService.Login_User(userDto.UserName, userDto.Password);
            if (isValid)
            {


                await localStrorage.SetItemAsStringAsync("username", userDto.UserName);
                AuthState.IsAuthenticated = true;
                Nav.NavigateTo($"/transaction");


            }
            else
            {
                isError = true;
                errorMessage = "Invalid username or password";
            }


        }

      


    }
}