using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public class LoginDialog : Dialog
    {
        TextBox name;
        PasswordBox password;
        public string UserName { get { return name.Text; } }
        public string UserPassword { get { return password.Text; } }

        public LoginDialog()
        {
            Title = "Login";

            Padding = 10;

            name = new TextBox();
            password = new PasswordBox { Width = 100 };

            Content = new StackLayout
            {
                Items =
                {
                    new Label { Text = "Name" },
                    name,
                    new Label { Text = "Password" },
                    password,
                }
            };

            // buttons
            DefaultButton = new Button { Text = "Login" };
            PositiveButtons.Add(DefaultButton);

            DefaultButton.Click += (sender, e) =>
            {
                string userName = name.Text;
                string userPassword = password.Text;
                Close();
            };

            AbortButton = new Button { Text = "C&ancel" };
            AbortButton.Click += (sender, e) => Close();
            NegativeButtons.Add(AbortButton);
        }
    }
}
