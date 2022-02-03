using MoonlapseServer;
using System;
using System.Threading;
using System.Threading.Tasks;

var server = new Server();
Task.Run(server.Start);
Thread.Sleep(-1);