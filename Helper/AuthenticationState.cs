

namespace BytesTracker.Helper
{
    public class AuthenticationState
    {
        private bool isAuthenticated;
        public event Action? OnAuthenticationChanged;

        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set
            {
                isAuthenticated = value;
                OnAuthenticationChanged?.Invoke();
            }
        }

    }
}
