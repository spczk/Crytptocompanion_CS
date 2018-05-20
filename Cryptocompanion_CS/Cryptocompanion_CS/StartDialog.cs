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
        public List<Wallet> GetWallets { get; set; }


        public StartDialog()
        {
            GetWallets = new List<Wallet>();

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
            AbortButton.Click += (sender, e) => Application.Instance.Quit();
            NegativeButtons.Add(AbortButton);

            DefaultButton.Click += (sender, e) =>
            {
                LoginCheck = false;
                //Opening a user specified file and reading it to the end
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
                    //If file isn't empty and user data is correct then it is decrypted
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

                            //Split encrypted file content into wallet objects and set decrypted properties, that are also a product of split
                                var objects = content[5].Split('*');
                                if (objects.Length > 0)
                                {

                                    foreach (var obj in objects)
                                    {
                                        var items = obj.Split('_');
                                        if (items.Length > 1)
                                        {
                                            Wallet wallet = new Wallet
                                            {
                                                Name = Cryptography.Decrypt(items[0], PassPhrase),
                                                Address = Cryptography.Decrypt(items[1], PassPhrase),
                                                CryptocurrencyName = Cryptography.Decrypt(items[2], PassPhrase),
                                                PassPhrase = Cryptography.Decrypt(items[3], PassPhrase),
                                                WordCode = Cryptography.Decrypt(items[4], PassPhrase),
                                                PublicKey = Cryptography.Decrypt(items[5], PassPhrase),
                                                PrivateKey = Cryptography.Decrypt(items[6], PassPhrase),
                                            };
                                            GetWallets.Add(wallet);
                                        }
                                    }
                                }
                            }
                        }
                    //Open a login dialog that authenticates user
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
