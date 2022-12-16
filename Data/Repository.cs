using Microsoft.EntityFrameworkCore;
using PhoneCentre.Models;

namespace CallCentreTask.Data
{
    public class Repository
    {
        public async static Task<List<T_Event>> GetEventAsync()
        {
            using (var db = new CallerDb())
            {
                return await db.Events
                    .Include(Event => Event.Call_)
                    .Include(Event => Event.Event_Type)
                    .ToListAsync();
            }
        }
        public async static Task<T_Event> GetEventByIdAsync(int id)
        {
            using (var db = new CallerDb())
            {
                return await db.Events
                    .Include(Event => Event.Call_)
                    .Include(Event => Event.Event_Type)
                    .FirstOrDefaultAsync(Event => Event.Record_Id == id);
            }
        }
        public async static Task<List<T_Event_Type>> GetEventTypeAsync()
        {
            using (var db = new CallerDb())
            {
                return await db.Events_Type.ToListAsync();
            }
        }
        public async static Task<T_Event_Type> GetEventTypeByIdAsync(string id)
        {
            using (var db = new CallerDb())
            {
                return await db.Events_Type.FirstOrDefaultAsync(Event => Event.Event_Id == id);
            }
        }
    }
}
