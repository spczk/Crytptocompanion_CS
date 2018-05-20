using System;
using Eto.Forms;
using Eto.Drawing;
using System.Collections.Generic;

namespace Cryptocompanion
{
    public partial class MainForm : Form
    {
        StartDialog start;
        List<Wallet> wallets;

        public MainForm()
        {
            wallets = new List<Wallet>();
            var grid = new GridView { DataStore = wallets};
            grid.GridLines = GridLines.Both;

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.Name) },
                HeaderText = "Name"
            });

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.CryptocurrencyName) },
                HeaderText = "Cryptocurrency"
            });

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.Address) },
                HeaderText = "Address"
            });

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.PublicKey) },
                HeaderText = "Public Key"
            });

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.PrivateKey) },
                HeaderText = "Private Key"
            });

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.PassPhrase) },
                HeaderText = "Pass Phrase"
            });

            grid.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<Wallet, string>(r => r.WordCode) },
                HeaderText = "Word Code"
            });


            start = new StartDialog();
            start.ShowModal(this);
            while (start.LoginCheck == false)
            {
                MessageBox.Show(this, "Wrong Name/Password. Try again.");
                start.ShowModal(this);
            }

            Title = "Cryptocompanion";
            ClientSize = new Size(550, 400);

            Content = grid;

            // create a few commands that can be used for the menu and toolbar
            var AddCommand = new Command { MenuText = "Add...", ToolBarText = "Add..." };
            AddCommand.Executed += (sender, e) => 
            {
                AddDialog add = new AddDialog();
                add.ShowModal(this);
                Wallet wallet = new Wallet
                {
                    Name = add.AddName,
                    Address = add.AddAddress,
                    CryptocurrencyName = add.AddCryptocurrencyName,
                    PassPhrase = add.AddPassPhrase,
                    PrivateKey = add.AddPrivateKey,
                    PublicKey = add.AddPublicKey,
                    WordCode = add.AddWordCode
                };
                wallets.Add(wallet);
                grid.DataStore = wallets;
            };

            var OpenCommand = new Command { MenuText = "Open...", ToolBarText = "Open..." };
            OpenCommand.Executed += (sender, e) => 
            {
                start.DefaultButton.PerformClick();
            };

            var SaveCommand = new Command { MenuText = "Save...", ToolBarText = "Save..." };
            SaveCommand.Executed += (sender, e) =>
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                if (saveFile.ShowDialog(this) == DialogResult.Ok)
                {
                    string fileName = saveFile.FileName;

                    string encRecoveryCode = Cryptography.Encrypt(start.user.RecoveryCode, start.PassPhrase);
                    string encFirstName = Cryptography.Encrypt(start.user.FirstName, start.PassPhrase);
                    string encLastName = Cryptography.Encrypt(start.user.LastName, start.PassPhrase);
                    string encPassword = Cryptography.Encrypt(start.user.Password, start.PassPhrase);

                    System.IO.File.WriteAllText(fileName, (start.PassPhrase + "-"));
                    System.IO.File.AppendAllText(fileName, (encFirstName + "-"));
                    System.IO.File.AppendAllText(fileName, (encLastName + "-"));
                    System.IO.File.AppendAllText(fileName, (encPassword + "-"));
                    System.IO.File.AppendAllText(fileName, encRecoveryCode);
                    MessageBox.Show(this, "File saved successfully!");
                }
            };

            var DeleteCommand = new Command { MenuText = "Delete...", ToolBarText = "Delete..." };
            DeleteCommand.Executed += (sender, e) => 
            {
                var row = grid.SelectedRow;
                if (row != -1)
                {
                    wallets.RemoveAt(row);
                    grid.DataStore = wallets;
                }
                else MessageBox.Show(this, "No items selected to delete");
            };

            var EditCommand = new Command { MenuText = "Edit...", ToolBarText = "Edit..." };
            EditCommand.Executed += (sender, e) =>
            {
                var row = grid.SelectedRow;
                if (row != -1)
                {
                    AddDialog add = new AddDialog
                    {
                        AddName = wallets[row].Name,
                        AddAddress = wallets[row].Address,
                        AddWordCode = wallets[row].WordCode,
                        AddPublicKey = wallets[row].PublicKey,
                        AddPassPhrase = wallets[row].PassPhrase,
                        AddPrivateKey = wallets[row].PrivateKey,
                        AddCryptocurrencyName = wallets[row].CryptocurrencyName,
                    };
                    add.ShowModal(this);
                    wallets[row].Name = add.AddName;
                    wallets[row].Address = add.AddAddress;
                    wallets[row].WordCode = add.AddWordCode;
                    wallets[row].PublicKey = add.AddPublicKey;
                    wallets[row].PassPhrase = add.AddPassPhrase;
                    wallets[row].PrivateKey = add.AddPrivateKey;
                    wallets[row].CryptocurrencyName = add.AddCryptocurrencyName;
                    grid.DataStore = wallets;
                }
                else MessageBox.Show(this, "No items selected to edit");
            };

            var ChangePasswordCommand = new Command { MenuText = "Change Password", ToolBarText = "Change Password" };
            ChangePasswordCommand.Executed += (sender, e) => 
            {
                ChangePasswordDialog change = new ChangePasswordDialog();
                change.ShowModal(this);
                if (change.RecoveryCodeCheck == start.user.RecoveryCode)
                {
                    start.user.Password = change.ChangedPassword;
                    MessageBox.Show(this, "Password changed successfully!");
                }

            };

            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

            // create menu
            Menu = new MenuBar
            {
                Items =
                {
					// File submenu
                    new ButtonMenuItem { Text = "&File", Items = { AddCommand, OpenCommand } },
					// new ButtonMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
					// new ButtonMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };

            // create toolbar			
            ToolBar = new ToolBar { Items = { AddCommand, SaveCommand, ChangePasswordCommand, OpenCommand, DeleteCommand, EditCommand } };
        }


    }
}
