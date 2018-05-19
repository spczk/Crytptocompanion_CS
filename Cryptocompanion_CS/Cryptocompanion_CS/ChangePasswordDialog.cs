using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public class ChangePasswordDialog : Dialog
    {

        TextBox RecoveryCode;
        TextBox NewPassword;
        public string RecoveryCodeCheck { get { return RecoveryCode.Text; } }
        public string ChangedPassword { get { return NewPassword.Text; } }

        public ChangePasswordDialog()
        {
            Title = "Change Password";
            Padding = 10;

            RecoveryCode = new TextBox();
            NewPassword = new TextBox();

            Content = new StackLayout
            {
                Items =
                {
                    new Label { Text = "Recovery Code" },
                    RecoveryCode,
                    new Label { Text = "New Password" },
                    NewPassword,
                }
            };

            // buttons
            DefaultButton = new Button { Text = "Change" };
            PositiveButtons.Add(DefaultButton);

            DefaultButton.Click += (sender, e) => 
            {
                Close();
            };

            AbortButton = new Button { Text = "C&ancel" };
            AbortButton.Click += (sender, e) => Close();
            NegativeButtons.Add(AbortButton);
        }
    }
}
