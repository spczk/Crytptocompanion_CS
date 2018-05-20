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
            start = new StartDialog();
            start.ShowModal(this);
            //Show the login dialog as long as the user authentication fails
            while (start.LoginCheck == false)
            {
                MessageBox.Show(this, "Wrong Name/Password. Try again.");
                start.ShowModal(this);
            }

            if (start.GetWallets.Count > 0)
            {
                wallets = start.GetWallets;
            }
            else wallets = new List<Wallet>();

            //Create a Grid View from a list of wallets
            var grid = new GridView { DataStore = wallets};
            grid.GridLines = GridLines.Both;
            //Adding columns to a grid view and binding data to them
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


            Title = "Cryptocompanion";
            ClientSize = new Size(550, 400);

            Content = grid;

            //Adding new wallet info capability
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
            //Open other file supported by program
            var OpenCommand = new Command { MenuText = "Open...", ToolBarText = "Open..." };
            OpenCommand.Executed += (sender, e) => 
            {
                start.GetWallets.Clear();
                start.DefaultButton.PerformClick();
                if (start.GetWallets.Count > 0)
                {
                    wallets = start.GetWallets;
                }
                else wallets = new List<Wallet>();
                grid.DataStore = wallets;
                start.Close();
            };
            //Save current data to  specified file
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
                    System.IO.File.AppendAllText(fileName, (encRecoveryCode + "-"));

                    foreach (var item in wallets)
                    {
                        //Encrypting and saving every wallet in list of wallets
                        string encItemName = Cryptography.Encrypt(item.Name, start.PassPhrase);
                        string encItemAddress = Cryptography.Encrypt(item.Address, start.PassPhrase);
                        string encItemCryptocurrencyName = Cryptography.Encrypt(item.CryptocurrencyName, start.PassPhrase);
                        string encItemPassPhrase = Cryptography.Encrypt(item.PassPhrase, start.PassPhrase);
                        string encItemWordCode = Cryptography.Encrypt(item.WordCode, start.PassPhrase);
                        string encItemPublicKey = Cryptography.Encrypt(item.PublicKey, start.PassPhrase);
                        string encItemPrivateKey = Cryptography.Encrypt(item.PrivateKey, start.PassPhrase);

                        System.IO.File.AppendAllText(fileName, (encItemName + "_"));
                        System.IO.File.AppendAllText(fileName, (encItemAddress + "_"));
                        System.IO.File.AppendAllText(fileName, (encItemCryptocurrencyName + "_"));
                        System.IO.File.AppendAllText(fileName, (encItemPassPhrase + "_"));
                        System.IO.File.AppendAllText(fileName, (encItemWordCode + "_"));
                        System.IO.File.AppendAllText(fileName, (encItemPublicKey + "_"));
                        System.IO.File.AppendAllText(fileName, (encItemPrivateKey + "*"));
                    }

                    MessageBox.Show(this, "File saved successfully!");
                }
            };
            //Delete wallet object
            var DeleteCommand = new Command { MenuText = "Delete...", ToolBarText = "Delete..." };
            DeleteCommand.Executed += (sender, e) => 
            {
                var row = grid.SelectedRow;
                //SelectedRow returns -1 when nothing is selected
                if (row != -1)
                {
                    wallets.RemoveAt(row);
                    grid.DataStore = wallets;
                }
                else MessageBox.Show(this, "No items selected to delete");
            };
            //Edit wallet info
            //Program displays add dialog with wallet data that is currently specified and then saves changes if any are done
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
            //Passowrd changing capability
            //User can change his password after entering a recovery code that is generated at the registration point
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
            //Closes the application
            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();


            // create menu
            Menu = new MenuBar
            {
                Items =
                {
					// File submenu
                    new ButtonMenuItem { Text = "&File", Items = { OpenCommand, ChangePasswordCommand, SaveCommand } },
                    new ButtonMenuItem { Text = "&Edit", Items = { AddCommand, EditCommand, DeleteCommand } },				},
                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
                },
                QuitItem = quitCommand,
            };

            // create toolbar			
            ToolBar = new ToolBar { Items = { OpenCommand, AddCommand, DeleteCommand, EditCommand, SaveCommand } };
        }


    }
}
