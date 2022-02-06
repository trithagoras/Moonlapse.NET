using System;
using System.Linq;
using MoonlapseServer.DbModels;
using MoonlapseNetworking;
using MoonlapseNetworking.Packets;
using MoonlapseServer.Utils.Logging;
using MoonlapseNetworking.ServerModels;
using MoonlapseNetworking.ServerModels.Components;
using MoonlapseServer.DbModels.Components;
using Microsoft.EntityFrameworkCore;

namespace MoonlapseServer.States
{
    public class EntryState : State
    {
        Protocol _protocol;

        bool _readyToChangeState;

        public EntryState(Protocol protocol)
        {
            _protocol = protocol;
            LoginPacketEvent += EntryState_LoginPacketEvent;
            RegisterPacketEvent += EntryState_RegisterPacketEvent;
            OkPacketEvent += EntryState_OkPacketEvent;
        }

        private void EntryState_OkPacketEvent(object sender, PacketEventArgs args)
        {
            if (_readyToChangeState)
            {
                _protocol.State = new GameState(_protocol);
            }
        }

        private void EntryState_RegisterPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<RegisterPacket>(args.PacketString);

            if (!IsStringWellFormed(p.Username) || !IsStringWellFormed(p.Password))
            {
                _protocol.Log($"Registration failed: username or password contains whitespace or is empty");
                _protocol.SendPacket(new DenyPacket { Message = "Fields cannot contain whitespace" });
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
                var e = new EntityDbModel
                {
                    Name = p.Username,
                    TypeName = "Player"
                };
                db.Add(new UserDbModel
                {
                    Username = p.Username,
                    Password = p.Password,
                    Entity = e
                });
                db.Add(new PositionComponent
                {
                    Component = new ComponentModel
                    {
                        Entity = e,
                        TypeName = "Position"
                    },
                    X = 0,
                    Y = 0
                });
                db.SaveChanges();
                _protocol.Log($"Registration successful: user with username={p.Username}");
                _protocol.SendPacket(new OkPacket { Message = "Register" });
            }
            else
            {
                // user already exists >:(
                _protocol.Log($"Registration failed: user with username={p.Username} already exists", LogContext.Warn);
                _protocol.SendPacket(new DenyPacket { Message = "User already exists" });
            }
        }

        void EntryState_LoginPacketEvent(object sender, PacketEventArgs args)
        {
            var p = Packet.FromString<LoginPacket>(args.PacketString);

            if (!IsStringWellFormed(p.Username) || !IsStringWellFormed(p.Password))
            {
                _protocol.Log($"Login failed: username or password contains whitespace or is empty");
                _protocol.SendPacket(new DenyPacket { Message = "Fields cannot contain whitespace" });
                return;
            }

            var db = new MoonlapseDbContext();

            _protocol.Log($"Attempting login with username={p.Username}");

            var user = db.Users
                .Include(u => u.Entity)
                .Where(u => u.Username == p.Username)
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
                    var entityModel = db.Entities
                        .Where(e => e == user.Entity)
                        .First();

                    var e = new Entity
                    {
                        Id = entityModel.Id,
                        Name = entityModel.Name,
                        TypeName = entityModel.TypeName
                    };

                    _protocol.Login(e);
                    _readyToChangeState = true;
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
