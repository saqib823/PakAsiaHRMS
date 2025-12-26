using Microsoft.AspNet.SignalR;

namespace hrms_PakAsia.Hubs
{
    public class AttendanceHub : Hub
    {
        // Server can call this method to notify clients
        public void BroadcastAttendance(string jsonData)
        {
            Clients.All.updateAttendance(jsonData);
        }
    }
}
