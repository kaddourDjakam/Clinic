using Clinik.Commands;
using Clinik.Model;
using Clinik.Repository.DataContext;
using Clinik.View.MainWindow;
using Clinik.ViewModel.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Clinik.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public static User? CurrentUser { get; private set; }
        private string username;
        private string password;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginAction);
        }

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }


        public ICommand LoginCommand { get; }

        private void LoginAction()
        {
            using (var dbContext = new ClinikEntities())
            {
                int userCount = dbContext.Users.Count();

                if (userCount < 1)
                {
                    // Create a new Person
                    Person newPerson = new Person
                    {
                        Fullname = "par défaut",
                        // ... other properties
                    };

                    // Create a new User
                    User newUser = new User
                    {
                        Username = "admin",
                        Password = "123",
                        // ... other properties
                    };

                    // Associate the User with the Person
                    newPerson.Users = new List<User> { newUser };

                    // Add the new Person to the database
                    dbContext.Persons?.Add(newPerson);

                    // Save changes to the database
                    dbContext.SaveChanges();


                }
                // Retrieve the user based on the provided username
                CurrentUser = dbContext.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

                if (CurrentUser != null)
                {
                    MainWindowView main = new MainWindowView() { DataContext = new MainWindowViewModel() };
                    Application.Current.MainWindow.Hide();
                    main.Show();

                }
                else
                {
                    // User does not exist
                    // Handle the scenario when the user is not found
                }


                // Implement the rest of your login logic here
            }
        }
    }
}
