﻿using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public class RegisterDialog : Dialog
    {


        public RegisterDialog()
        {
            Title = "Register";
            TextBox firstName = new TextBox();
            TextBox lastName = new TextBox();
            PasswordBox password = new PasswordBox{Width = 100};

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    new Label { Text = "First Name" },
                    firstName,
                    new Label { Text = "Last Name"},
                    lastName,
                    new Label { Text = "Password"},
                    password,

                }
            };


            // buttons
            DefaultButton = new Button { Text = "Register" };
            PositiveButtons.Add(DefaultButton);

            DefaultButton.Click += (sender, e) =>
            {
                //Opening a save file dialog
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog(this) == DialogResult.Ok)
                {
                    //After dialog is closed program encrypts user data and writes it to the specified file
                    string fileName = saveFile.FileName;
                    string passPhrase = Cryptography.GetUniqueKey(6);
                    string recoveryCode = Cryptography.GetRecoveryCode(6);
                    string encRecoveryCode = Cryptography.Encrypt(recoveryCode, passPhrase);
                    string encFirstName = Cryptography.Encrypt(firstName.Text, passPhrase);
                    string encLastName = Cryptography.Encrypt(lastName.Text, passPhrase);
                    string encPassword = Cryptography.Encrypt(password.Text, passPhrase);

                    System.IO.File.WriteAllText(fileName, (passPhrase + "-"));
                    System.IO.File.AppendAllText(fileName, (encFirstName + "-"));
                    System.IO.File.AppendAllText(fileName, (encLastName + "-"));
                    System.IO.File.AppendAllText(fileName, (encPassword + "-"));
                    System.IO.File.AppendAllText(fileName, (encRecoveryCode + "-"));
                    MessageBox.Show(this, "Registered Succesfully!\nYour recovery code is: " + recoveryCode);
                }
                Close();
                };

            AbortButton = new Button { Text = "C&ancel" };
            AbortButton.Click += (sender, e) => Close();
            NegativeButtons.Add(AbortButton);
        }
    }
}
