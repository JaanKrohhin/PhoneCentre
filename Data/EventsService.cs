using CallCentreTask.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PhoneCentre.Models;
using System.Linq.Expressions;
using System.Reflection;

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

                return db.Events.Include(Event => Event.Call_)

                    .Include(Event => Event.Event_Type)

                    .FilterByEventType(eventTypefilter)

                    .GetPage(size, numberOfSkips);
        }
        /*
         * "Try implementing CSV export in a way that the data is streamed to user from the database, not all loaded into memory first"
         * 
         * I believe I have improved it but I have no way to know since dealing with memory has never been never in my mindset for studies, only when I was writing code for Arduino Uno with its small memory. 
         */
        public List<T_Event> GetCSVData(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, int chunkSkip, int dataChunkSize)
        {
            var query = db.Events.Include(Event => Event.Call_).Include(Event => Event.Event_Type);

            // apply filters and sort
            return Apply_Sorting_And_Filtering_To_IQueryable_For_CSV(query, sortColumn, searchString, sortDirection, eventTypefilter, chunkSkip, dataChunkSize).ToList();

        }



        public IQueryable<T_Event> Apply_Sorting_And_Filtering_To_IQueryable_For_CSV(IQueryable<T_Event> query, string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, int chunkSkip, int dataChunkSize)
        {

            return query.SortByColumn(sortColumn, sortDirection)

                .FilterBySearch(searchString)

                .FilterByEventType(eventTypefilter)

                .GetPage(dataChunkSize, chunkSkip);

        }
        public IQueryable<T_Event> SortByColumn(IQueryable<T_Event> query, string columnName, string columnDirection)
        {
            //Determine in which order the columns will go

            return query.SortByColumn(columnName, columnDirection);

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
            return db.Events.Where(Event => Event.Call_.Caller == caller)
                                .Include(Event => Event.Call_)
                                .Select(Event => Event.Call_Id)
                                .Distinct()
                                .ToArray();
        }
    }
}
