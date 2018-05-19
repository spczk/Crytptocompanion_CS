using System;
using Eto.Forms;
using Eto.Drawing;

namespace Cryptocompanion
{
    public partial class MainForm : Form
    {
        private StartDialog start;
        public MainForm()
        {
            start = new StartDialog();
            start.ShowModal(this);
            if (start.LoginCheck == false)
            {
                Close();
            }

            Title = "Cryptocompanion";
            ClientSize = new Size(400, 350);

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
				}
            };

            // create a few commands that can be used for the menu and toolbar
            var AddCommand = new Command { MenuText = "Add...", ToolBarText = "Add..." };
            AddCommand.Executed += (sender, e) => 
            {
                AddDialog add = new AddDialog();
                add.ShowModal(this);
            };

            var OpenCommand = new Command { MenuText = "Open...", ToolBarText = "Open..." };
            OpenCommand.Executed += (sender, e) => 
            {
                
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
                    new ButtonMenuItem { Text = "&File", Items = { AddCommand } },
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
            ToolBar = new ToolBar { Items = { AddCommand, SaveCommand, ChangePasswordCommand } };
        }


    }
}
