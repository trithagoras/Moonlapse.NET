using System;
using System.Linq;
using MoonlapseServer.Models;
using MoonlapseServer.Models.Packets;
using MoonlapseServer.Utils.Logging;

namespace MoonlapseServer.States
{
    public class EntryState : State
    {
        public EntryState(Protocol protocol) : base(protocol)
        {
        }

        protected override void HandleLoginPacket(LoginPacket p)
        {
            if (!IsStringWellFormed(p.Username) || !IsStringWellFormed(p.Password))
            {
                _protocol.Log($"Login failed: username or password contains whitespace or is empty");
                return;
            }

            var db = new MoonlapseDbContext();

            _protocol.Log($"Attempting login with username={p.Username}");

            var user = db.Users
                .Where(e => e.Username == p.Username)
                .FirstOrDefault();

            if (user == null)
            {
                // user does not exist
                _protocol.Log($"Login failed: user with username={p.Username} does not exist");
            }
            else
            {
                // user exists
                if (user.Password == p.Password)
                {
                    _protocol.Login(p.Username);
                    _protocol.State = new MainState(_protocol);
                }
                else
                {
                    // incorrect password
                    _protocol.Log($"Login failed: password incorrect");
                }
            }
        }

        protected override void HandleRegisterPacket(RegisterPacket p)
        {
            if (!IsStringWellFormed(p.Username) || !IsStringWellFormed(p.Password))
            {
                _protocol.Log($"Registration failed: username or password contains whitespace or is empty");
                return;
            }

            var db = new MoonlapseDbContext();

            _protocol.Log($"Attempting registration with username={p.Username}");

            var user = db.Users
                .Where(e => e.Username == p.Username)
                .FirstOrDefault();

            if (user == null)
            {
                // can register :)
                db.Add(new UserModel
                {
                    Username = p.Username,
                    Password = p.Password,
                    Entity = new EntityModel
                    {
                        Name = p.Username,
                        TypeName = "Player"
                    }
                });
                db.SaveChanges();
                _protocol.Log($"Registration successful: user with username={p.Username}");
            }
            else
            {
                // user already exists >:(
                _protocol.Log($"Registration failed: user with username={p.Username} already exists", LogContext.Warn);
            }
        }

        /// <summary>
        /// A string to be used in usernames + passwords should not contain spaces or be empty
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static bool IsStringWellFormed(string s) => !(s.Contains(' ') || s == "");
    }
}
