using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public class StartDialog : Dialog
    {
        private Button RegisterButton;
        public User user { get; set; }
        public bool LoginCheck { get; set; }
        public string PassPhrase { get; set; }

        public StartDialog()
        {
            Title = "Cryptocompanion";

            LoginCheck = false;

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
            AbortButton.Click += (sender, e) => Application.Instance.Quit();
            NegativeButtons.Add(AbortButton);

            DefaultButton.Click += (sender, e) =>
            {
                user = new User();
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog(this) == DialogResult.Ok)
                {
                    string fileName;
                    fileName = fileDialog.FileName;
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
                                PassPhrase = content[0];
                                string decFirstName = Cryptography.Decrypt(content[1], PassPhrase);
                                string decLastName = Cryptography.Decrypt(content[2], PassPhrase);
                                string decPassword = Cryptography.Decrypt(content[3], PassPhrase);
                                string decRecoveryCode = Cryptography.Decrypt(content[4], PassPhrase);
                                user.FirstName = decFirstName;
                                user.LastName = decLastName;
                                user.Password = decPassword;
                                user.RecoveryCode = decRecoveryCode;
                            }
                        }
                        LoginDialog login = new LoginDialog();
                        login.ShowModal(this);
                            if (login.UserName == (user.FirstName + " " + user.LastName) && login.UserPassword == user.Password)
                            {
                                MessageBox.Show(this, "Login Succesful!");
                                LoginCheck = true;
                            }
                    Close();
                }
             
            };

            RegisterButton.Click += (sender, e) => new RegisterDialog().ShowModal();
        }
    }

}
