using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public class StartDialog : Dialog
    {
        private Button RegisterButton;
        private User user;

        public StartDialog()
        {
            Title = "Cryptocompanion";

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                }
            };

            // buttons
            DefaultButton = new Button { Text = "Login" };
            RegisterButton = new Button { Text = "Register" };
            PositiveButtons.Add(DefaultButton);
            PositiveButtons.Add(RegisterButton);

            AbortButton = new Button { Text = "Quit" };
            AbortButton.Click += (sender, e) => Close();
            NegativeButtons.Add(AbortButton);

            DefaultButton.Click += (sender, e) =>
            {
                user = new User();
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog(this) == DialogResult.Ok)
                {
                    string fileName;
                    fileName = fileDialog.FileName;
                    MessageBox.Show(this, fileName);
                        string fileContent;
                        using (System.IO.StreamReader sr = System.IO.File.OpenText(fileName))
                        {
                            fileContent = sr.ReadToEnd();
                        }
                        if (!String.IsNullOrEmpty(fileContent))
                        {
                            var content = fileContent.Split('-');
                            if (content.Length > 1)
                            {
                                user.firstName = content[0];
                                user.lastName = content[1];
                                user.password = content[2];
                            }
                        }
                        LoginDialog login = new LoginDialog();
                        login.ShowModal(this);
                            if (login.UserName == (user.firstName + " " + user.lastName) && login.UserPassword == user.password)
                            {
                                MessageBox.Show(this, "Login Succesful!");
                            }

                }
             
            };

            RegisterButton.Click += (sender, e) => new RegisterDialog().ShowModal();
        }
    }
}
