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
        public IEnumerable<T_Event> GetData(string searchString, string[] eventTypefilter, string sortColumn, string sortDirection, int numberOfSkips, int size)
        {

                return db.Events.Include(Event => Event.Call_)

                    .Include(Event => Event.Event_Type)

                    .FilterByEventType(eventTypefilter)

                    .FilterBySearch(searchString)
                    
                    .SortByColumn(sortColumn, sortDirection)

                    .GetPage(size, numberOfSkips);
        }




        public IEnumerable<T_Event> GetCSVData(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, int chunkSkip)
        {

            //Percentage of all the stored Events in decimals to get
            var chunkPercentage = 0.1;


            //The chunk size
            int dataChunkSize = (int)(db.Events.Count() * chunkPercentage);



            var query = GetData(searchString, eventTypefilter,  sortColumn, sortDirection, chunkSkip, dataChunkSize).AsEnumerable();

            
            
            return query;

        }



        
        public IEnumerable<T_Event> SortByColumn(IEnumerable<T_Event> query, string columnName, string columnDirection)
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
