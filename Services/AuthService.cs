using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DigiDineKDS.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        bool IsAuthenticated { get; }
        string Role { get; }
        event Action? OnLoginStateChanged;
    }

    public class AuthService : IAuthService
    {
        public bool IsAuthenticated { get; private set; }
        public string Role { get; private set; }

        public event Action? OnLoginStateChanged;
        public Task<bool> LoginAsync(string username, string password)
        {
            // Simple hardcoded logic for demo purposes
            if (username == "kitchen" && password == "kitchen")
            {
                Role = "Kitchen";
                IsAuthenticated = true;
            }
            //else if (username == "cashier" && password == "cashier")
            //{
            //    Role = "Cashier";
            //    IsAuthenticated = true;
            //}
            else
            {
                IsAuthenticated = false;
            }

            OnLoginStateChanged?.Invoke();
            return Task.FromResult(IsAuthenticated);
        }

    }


}
