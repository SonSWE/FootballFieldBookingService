using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace FootballFieldBookingService
{
    public interface IWebsocketHandler
    {
        Task Handle(Guid id, WebSocket websocket);
    }
}