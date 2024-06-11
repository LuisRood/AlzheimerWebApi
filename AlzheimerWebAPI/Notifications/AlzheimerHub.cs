using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace AlzheimerWebAPI.Notifications
{
    [Authorize]
    public class AlzheimerHub : Hub
    {
        private static readonly ConcurrentDictionary<string,HashSet<string>> _activeDevices = new();
        public async Task SubscribeToDevices(string[] deviceIds)
        {
            var connectionId = Context.ConnectionId;

            foreach(var deviceId in deviceIds)
            {
                if (_activeDevices.ContainsKey(deviceId))
                {
                    _activeDevices[deviceId].Add(deviceId);
                }
                else
                {
                    _activeDevices[deviceId] = new HashSet<string> {connectionId};
                }
                await Groups.AddToGroupAsync(connectionId, deviceId);
                Console.WriteLine("Se suscribio la MAC: "+deviceId);
            }
        }

        public async Task UnsubscribeFromDevices(string[] deviceIds)
        {
            var connectionId = Context.ConnectionId;

            foreach (var deviceId in deviceIds)
            {
                if (_activeDevices.ContainsKey(deviceId))
                {
                    _activeDevices[deviceId].Remove(deviceId);
                    if (_activeDevices[deviceId].Count == 0) 
                    {
                        _activeDevices.TryRemove(connectionId, out _);
                    }
                }
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceId);
            }
        }
        public static IReadOnlyDictionary<string, HashSet<string>> GetActiveDevices() => _activeDevices;

    }
}
