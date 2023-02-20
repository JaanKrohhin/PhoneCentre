using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PhoneCentre.Data.Databases;
using PhoneCentre.Data.Interfaces;
using PhoneCentre.Models;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Text;

namespace PhoneCentre.Data.Services
{

    // "Implement proper code separation: Controllers must never access the database directly, instead they need to call a service class, which handles the database operations"


    public class MongoService : IService
    {

        private readonly MongoDb db;

        public MongoService(IOptions<DbOptions> options)
        {
            db = new MongoDb(options.Value);
        }


        //Sorts the data in ASC/DESC order by Caller/Receiver
        /* 
           "Pagination needs to be done in the database query, only the data needed for the current page must be fetched"
        
            The data does fetch only the required page using Skip() and Take() methods, however because I am fetching only one page, only that one page is sorted. 
            When you list through the pages you will notice that they dont follow the sort as in, it works for the page, but not between pages. For example, you sort by the receiver number in descending number. The highest item on each page will(probably) be a number with 8xxxxx, and the lowest 3xxxxx (or none at all).
            I have no clue, if this is the correct way to interpet sorting said by the task.
        
         */
        public IQueryable<T_Event> GetData(string searchString, string[] eventTypefilter, string sortColumn, string sortDirection, int numberOfSkips, int size)
        {
            return db.GetEventTableAsQueryable()

                    .FilterByEventType(eventTypefilter)

                    .FilterBySearch(searchString)

                    .SortByColumn(sortColumn, sortDirection)

                    .GetPage(size, numberOfSkips);
        }




        public async void WriteCSVDataToStream(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, PipeWriter writer)
        {

            var query = db.GetEventTableAsQueryable()

                             .FilterByEventType(eventTypefilter)

                             .FilterBySearch(searchString)

                             .SortByColumn(sortColumn, sortDirection);



            writer.WriteAsync(Encoding.ASCII.GetBytes("Caller,Event,Receiver,Timestamp\n"));
            foreach (var item in query)
            {
                writer.WriteAsync(Encoding.ASCII.GetBytes($"{item.FormatToCvsString()}\n"));
            }

        }

        public T_Event[] GetCall(int callId)
        {
            return db.GetEventTableAsQueryable()
                .Where(Event => Event.Call_Id == callId)
                .ToArray();
        }

        public int?[] GetCallHistory(int caller)
        {
            return db.GetEventTableAsQueryable()
                .Where(Event => Event.Call_.Caller == caller)
                                .Select(Event => Event.Call_Id)
                                .Distinct()
                                .ToArray();
        }
    }
}
