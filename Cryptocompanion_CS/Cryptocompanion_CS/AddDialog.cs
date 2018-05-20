using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public class AddDialog : Dialog
    {

        TextBox Name;
        TextBox CryptocurrencyName;
        TextBox Address;
        TextBox PublicKey;
        TextBox PrivateKey;
        TextBox PassPhrase;
        TextBox WordCode;
        public string AddName { get { return Name.Text; } set { Name.Text = value; } }
        public string AddCryptocurrencyName { get { return CryptocurrencyName.Text; } set { CryptocurrencyName.Text = value; } }
        public string AddAddress { get { return Address.Text; } set { Address.Text = value; } }
        public string AddPublicKey { get { return PublicKey.Text; } set { PublicKey.Text = value; } }
        public string AddPrivateKey { get { return PrivateKey.Text; } set { PrivateKey.Text = value; } }
        public string AddPassPhrase { get { return PassPhrase.Text; } set { PassPhrase.Text = value; } }
        public string AddWordCode { get { return WordCode.Text; } set { WordCode.Text = value; } }

        public AddDialog()
        {
            Title = "Add";
            Padding = 10;

            Name = new TextBox();
            CryptocurrencyName = new TextBox();
            Address = new TextBox();
            PublicKey = new TextBox();
            PrivateKey = new TextBox();
            PassPhrase = new TextBox();
            WordCode = new TextBox();

            Content = new StackLayout
            {
                Items =
                {
                    new Label { Text = "Name" },
                    Name,
                    new Label { Text = "Cryptocurrency Name" },
                    CryptocurrencyName,
                    new Label { Text = "Address" },
                    Address,
                    new Label { Text = "Public Key" },
                    PublicKey,
                    new Label { Text = "Private Key" },
                    PrivateKey,
                    new Label { Text = "Pass Phrase" },
                    PassPhrase,
                    new Label { Text = "Word Code" },
                    WordCode,
                }
            };

            // buttons
            DefaultButton = new Button { Text = "OK" };
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
