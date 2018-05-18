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
            start.Shown += Start_Shown;
            Title = "My Eto Form";
            ClientSize = new Size(400, 350);
            Opacity = 20;

            Content = new StackLayout
            {
                Padding = 10,
                Items =
                {
                    "Hello World!",
					// add more controls here
				}
            };

            // create a few commands that can be used for the menu and toolbar
            var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
            clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

            var clickMeToo = new Command { MenuText = "Click Me Too!", ToolBarText = "Click Me Too!" };
            clickMeToo.Executed += (sender, e) =>
            {
                new StartDialog().ShowModal(this);
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
					new ButtonMenuItem { Text = "&File", Items = { clickMe } },
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
            ToolBar = new ToolBar { Items = { clickMe, clickMeToo } };
        }

        void Start_Shown(object sender, EventArgs e)
        {
            Opacity = 0;
        }

    }
}
