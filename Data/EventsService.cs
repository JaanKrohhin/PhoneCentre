using CallCentreTask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhoneCentre.Models;
using System.Linq.Expressions;

namespace PhoneCentre.Data
{

    // "Implement proper code separation: Controllers must never access the database directly, instead they need to call a service class, which handles the database operations"


    public class EventService
    {

        private readonly CallerDb db;

        public EventService()
        {
            db = new CallerDb();
        }


        //Sorts the data in ASC/DESC order by Caller/Receiver
        /* 
           "Pagination needs to be done in the database query, only the data needed for the current page must be fetched"
        
            The data does fetch only the required page using Skip() and Take() methods, however because I am fetching only one page, only that one page is sorted. 
            When you list through the pages you will notice that they dont follow the sort as in, it works for the page, but not between pages. For example, you sort by the receiver number in descending number. The highest item on each page will(probably) be a number with 8xxxxx, and the lowest 3xxxxx (or none at all).
            I have no clue, if this is the correct way to interpet sorting said by the task.
        
         */
        public IQueryable<T_Event> GetDataFilteredByEvent(int numberOfSkips, int size, string[] eventTypefilter)
        {


            if (eventTypefilter.Any(EventType => EventType != ""))
            {

                return db.Events.Include(Event => Event.Call_)

                    .Include(Event => Event.Event_Type)

                    .Where(Event => eventTypefilter.Any(type => Event.Event_Type.Event_Id.Trim() == type))

                    .Skip((numberOfSkips - 1) * size)

                    .Take(size).AsQueryable();

            }

            return db.Events.Include(Event => Event.Call_)

                    .Include(Event => Event.Event_Type)

                    .Skip((numberOfSkips - 1) * size)

                    .Take(size).AsQueryable();

        }
        /*
         * "Try implementing CSV export in a way that the data is streamed to user from the database, not all loaded into memory first"
         * 
         * I believe I have improved it but I have no way to know since dealing with memory has never been never in my mindset for studies, only when I was writing code for Arduino Uno with its small memory. 
         */
        public IQueryable<T_Event> GetCSVData(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter)
        {
            var query = db.Events.Include(Event => Event.Call_).Include(Event => Event.Event_Type);

            // apply filters and sort
            return Apply_Sorting_And_Filtering_To_IQueryable_For_CSV(query, sortColumn, searchString, sortDirection, eventTypefilter);

        }



        public IQueryable<T_Event> Apply_Sorting_And_Filtering_To_IQueryable_For_CSV(IQueryable<T_Event> query, string sortColumn, string searchString, string sortDirection, string[] eventTypefilter)
        {

            // Apply sorting
            query = SortByColumn(query, sortColumn, sortDirection);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = FilterBySearch(query, searchString);
            }

            // Apply filtering
            if (eventTypefilter.Any(EventType => EventType != ""))
            {
                query = FilterByEventType(query, eventTypefilter);
            }



            return query;
        }
        public IQueryable<T_Event> SortByColumn(IQueryable<T_Event> query, string columnName, string columnDirection)
        {
            //Determine in which order the columns will go
            var ascending = columnDirection == "asc";


            //Sort by the column using the column name as a key in a dictionary
            if (EventServiceHelpers._sortColumns.TryGetValue(columnName, out var sortBy))
            {
                return (ascending ? query.OrderBy(sortBy) : query.OrderByDescending(sortBy)).AsQueryable();
            }
            return Array.Empty<T_Event>().AsQueryable();

        }
        public IQueryable<T_Event> FilterByEventType(IQueryable<T_Event> query, string[] eventTypefilter)
        {
            query = query.Where(_event => eventTypefilter.Contains(_event.Event_Type.Event_Id));
            return query;
        }

        public IQueryable<T_Event> FilterBySearch(IQueryable<T_Event> query, string searchString)
        {
            query = query.Where(e => e.Call_.Caller_.ToString().StartsWith(searchString) || e.Call_.Receiver.ToString().StartsWith(searchString));
            return query;
        }

        public T_Event[] GetCall(int callId)
        {
            return db.Events.Where(Event => Event.Call_Id == callId)
                .Include(Event => Event.Call_)
                .Include(Event => Event.Event_Type)
                .ToArray();
        }

        public int?[] GetCallHistory( int caller)
        {
            return db.Events.Where(Event => Event.Call_.Caller_ == caller)
                                .Include(Event => Event.Call_)
                                .Select(Event => Event.Call_Id)
                                .Distinct()
                                .ToArray();
        }
    }
}
