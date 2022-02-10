using System;
using Microsoft.Xna.Framework;
using SadConsole.Controls;
using MoonlapseClient.States;
using MoonlapseNetworking.Packets;

namespace MoonlapseClient.Consoles
{
    public class EntryConsole : SadConsole.ControlsConsole
    {
        readonly EntryState _state;

        readonly TextBox UsernameTextBox, PasswordTextBox;
        readonly Button RegisterButton, LoginButton;
        readonly Label ErrorLabel;

        public EntryConsole(EntryState state) : base(Game.Width, Game.Height, FontController.TextFont)
        {
            _state = state;

            ThemeColors = SadConsole.Themes.Library.Default.Colors;
            ThemeColors.ControlBack = Color.Black;

            UsernameTextBox = new TextBox(20)
            {
                Position = new Point(15, 8)
            };
            Add(UsernameTextBox);

            PasswordTextBox = new TextBox(20)
            {
                Position = new Point(15, 10),
                PasswordChar = "*"
            };
            Add(PasswordTextBox);

            RegisterButton = new Button(10)
            {
                Text = "Register",
                Position = new Point(4, 14)
            };
            Add(RegisterButton);
            RegisterButton.Click += RegisterButton_Click;

            LoginButton = new Button(7)
            {
                Text = "Login",
                Position = new Point(20, 14)
            };
            Add(LoginButton);
            LoginButton.Click += LoginButton_Click;

            ErrorLabel = new Label(50)
            {
                Position = new Point(4, 6),
                TextColor = Color.Red,
            };
            SetErrorLabel(_state.ErrorMessage);
            Add(ErrorLabel);

            ThemeColors.RebuildAppearances();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // client-side validation first
            if (!IsStringWellFormed(UsernameTextBox.Text) || !IsStringWellFormed(PasswordTextBox.Text))
            {
                SetErrorLabel("Fields cannot contain whitespace");
                return;
            }

            // send to server
            var registerPacket = new RegisterPacket
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Text
            };
            _state.Register(registerPacket);
        }

        void LoginButton_Click(object sender, EventArgs e)
        {
            // client-side validation first
            if (!IsStringWellFormed(UsernameTextBox.Text) || !IsStringWellFormed(PasswordTextBox.Text))
            {
                SetErrorLabel("Fields cannot contain whitespace");
                return;
            }

            // send to server
            var loginPacket = new LoginPacket
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Text
            };
            _state.Login(loginPacket);
        }

        protected override void OnInvalidate()
        {
            base.OnInvalidate();

            var colors = ThemeColors ?? SadConsole.Themes.Library.Default.Colors;

            Print(4, 4, "Welcome to Moonlapse!");

            Print(4, 8, "Username: ", colors.White);

            Print(4, 10, "Password: ", colors.White);
        }

        public void SetErrorLabel(string s, bool error = true)
        {
            ErrorLabel.TextColor = error ? ThemeColors.Red : ThemeColors.Cyan;
            ErrorLabel.DisplayText = s;
        }

        /// <summary>
        /// A string to be used in usernames + passwords should not contain spaces or be empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static bool IsStringWellFormed(string s) => !(s.Contains(' ') || s == "");
    }
}
