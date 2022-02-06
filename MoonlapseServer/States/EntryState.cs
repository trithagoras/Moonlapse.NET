using System;
using System.Linq;
using MoonlapseServer.Models;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseServer.Utils.Logging;

namespace MoonlapseServer.States
{
    public class EntryState : State
    {
        Protocol _protocol;

        public EntryState(Protocol protocol)
        {
            _protocol = protocol;
            LoginPacketEvent += EntryState_LoginPacketEvent;
            RegisterPacketEvent += EntryState_RegisterPacketEvent;
        }

        private void EntryState_RegisterPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<RegisterPacket>(args.PacketString);

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

        void EntryState_LoginPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<LoginPacket>(args.PacketString);

            if (!IsStringWellFormed(p.Username) || !IsStringWellFormed(p.Password))
            {
                _protocol.Log($"Login failed: username or password contains whitespace or is empty");
                _protocol.SendPacket(new DenyPacket
                {
                    Message = "Fields cannot contain whitespace"
                });
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
                _protocol.SendPacket(new DenyPacket
                {
                    Message = "User does not exist or incorrect password"
                });
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
                    _protocol.SendPacket(new DenyPacket
                    {
                        Message = "User does not exist or incorrect password"
                    });
                }
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
