using CallCentreTask.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using PhoneCentre.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace PhoneCentre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        #region Http get methods of the API

        
        /// <summary>
        /// Gets all events from the database and returns them sorted by the parameters
        /// </summary>
        /// <param name="rowSize">The current size of the row. Needed to send only the required data to the client and not all of it.</param>
        /// <param name="pageNumber">The current number of the page on the client. Needed to send the correct data.</param>
        /// <param name="sortColumn">Column that the user wants to sort by. Only can sort by caller and receiver ASC/DESC</param>
        /// <param name="optionalParams">Optional parameters that are used for sorting and filtering the data, separated by '+' sign. Direction of the sort: ASC/DESC. The search string to filter by. The events to filter by. A full variable will look like this: "desc+375+EVENT_PICK_UP----"</param>
        /// <returns>Filtered and sorted data from DB as an Array</returns>
        [HttpGet("{rowSize:int}/{pageNumber:int}/{sortColumn}/{optionalParams?}")]
        public T_Event[] Index(int rowSize,int pageNumber, string sortColumn, string optionalParams = "asc")
        {
            // Split the optional parameters into their respective variables
            string searchString, sortDirection;
            string[] eventTypefilter;
            ConvertParamsToVariables(optionalParams, out searchString, out sortDirection, out eventTypefilter);

            //Get data array
            T_Event[] eventsArray = GetFilteredAndSortedData(sortColumn, searchString, sortDirection, eventTypefilter);
            int numberFullOfPages = eventsArray.Length / rowSize;

            //Check if this is the last page of the data, if yes, return the last row
            if (pageNumber == numberFullOfPages + 1)
            {
                List<T_Event> lastRow = new List<T_Event> { };
                for (int i = rowSize * numberFullOfPages; i < eventsArray.Length; i++)
                {
                    var eventItem = eventsArray[i];
                    lastRow.Add(eventItem);
                }
                return lastRow.ToArray();
            }
            else if (pageNumber > numberFullOfPages + 1)
            {
                return new T_Event[] { };
            }

            //Returns the selected page of the data
            return eventsArray.AsSpan().Slice((pageNumber - 1) * rowSize, rowSize).ToArray();

        }



        
        /// <summary>
        /// Gets full details of the event by its Call ID
        /// </summary>
        /// <param name="callId">Id of the call to get the details of</param>
        /// <returns>An array of events that all have same callId</returns>
        [HttpGet("details/{callId:int}")]
        public T_Event[] CallDetails(int callId)
        {
            using (var db = new CallerDb())
            {
                var query = db.Events.Where(Event => Event.Call_Id == callId)
                    .Include(Event => Event.Call_)
                    .Include(Event => Event.Event_Type)
                    .ToArray();
                return query;
            }
        }




        /// <summary>
        /// Sorts and filters the data to a file with .csv extension for download
        /// </summary>
        /// <param name="sortColumn">Column to sort by</param>
        /// <param name="sortParams">Parameters that are used for sorting and filtering the data, separated by '+' sign. Direction of the sort: ASC/DESC. The search string to filter by. The events to filter by. A full variable will look like this: "desc+375+EVENT_PICK_UP----"</param>
        /// <returns>A downloadable file with the todays date in the name: "all_records_yyyyMMdd.csv"</returns>
        [HttpGet("download/{sortColumn}/{sortParams}")]
        public FileResult DownloadCSV(string sortColumn, string sortParams)
        {
            
            string searchString, sortDirection;
            string[] eventTypefilter;
            ConvertParamsToVariables(sortParams, out searchString, out sortDirection, out eventTypefilter);
            T_Event[] eventsArray = GetFilteredAndSortedData(sortColumn, searchString, sortDirection, eventTypefilter);


            //Building the csv file
            var csv = new StringBuilder();

            //Adding the headers
            csv.AppendLine("Caller,Event,Receiver,Timestamp");

            //Adding the data with the correct format
            foreach (var eventItem in eventsArray)
            {
                csv.AppendLine(eventItem.FormatToCvsString());
            }

            //Converting to byte array
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(csv.ToString());

            //Returning the file
            return File(byteArray, "text/csv", $"all_records_{DateTime.Now.ToString("yyyyMMdd")}.csv");
        }

        /// <summary>
        /// Returns the results of all the calls associated with the caller
        /// </summary>
        /// <param name="caller">Number of the caller</param>
        /// <returns>Returns a jagged array of events with the same caller number</returns>
        [HttpGet("history/{caller:int}")]
        public T_Event[][] HistoryDetails(int caller)
        {
            using (var db = new CallerDb())
            {
                var query = db.Events.Where(Event => Event.Call_.Caller_ == caller)
                    .Include(Event => Event.Call_)
                    .Select(Event => Event.Call_Id)
                    .Distinct()
                    .ToArray();
                T_Event[][] historyOfCalls = new T_Event[query.Count()][];
                for (int i = 0; i < query.Count(); i++)
                {
                    historyOfCalls[i] = db.Events.Where(Event => Event.Call_Id == query[i])
                        .Include(Event => Event.Call_)
                        .Include(Event => Event.Event_Type)
                        .OrderByDescending(Event => Event.Record_Date)
                        .ToArray();
                }

                historyOfCalls = SortJaggedArrayByDatetime(historyOfCalls);

                return historyOfCalls;
            }
        }
        #endregion

        #region Methods for sorting and filtering the database to the desired needs


        
        private T_Event[] GetFilteredAndSortedData(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter)
        {
            T_Event[] eventsArray = GetSortedArrayByColumn(sortColumn, sortDirection);
            if (searchString != "")
            {
                eventsArray = FilterArrayBySearch(eventsArray, searchString);
            }
            if (eventTypefilter.Count() > 0 && eventTypefilter.Any(Type => Type != ""))
            {
                eventsArray = FilterArrayByType(eventsArray, eventTypefilter);
            }

            return eventsArray;
        }

        //Filters the data by event IDs 
        private T_Event[] FilterArrayByType(T_Event[] eventsArray, string[] eventTypefilter)
        {
            return eventsArray.Where(Event => eventTypefilter.Any(type => Event.Event_Type.Event_Id.Trim() == type)).ToArray();
        }

        //Filters the data by the search string
        private T_Event[] FilterArrayBySearch(T_Event[] eventsArray, string searchString)
        {
            return eventsArray.Where(Event => Event.Call_.Receiver.ToString().StartsWith(searchString) || Event.Call_.Caller_.ToString().StartsWith(searchString)).ToArray();
        }


        //Sorts the data in ASC/DESC order by Caller/Receiver
        private T_Event[] GetSortedArrayByColumn(string sortcolumn,string columnDirection)
        {
            using (var db = new CallerDb())
            {
                var query = db.Events.Include(Event => Event.Call_).Include(Event => Event.Event_Type).ToList();

                if (sortcolumn == "caller" && columnDirection == "asc")
                {
                    
                    query.Sort((a, b) => a.Call_.Caller_.CompareTo(b.Call_.Caller_));

                }
                else if (sortcolumn == "caller" && columnDirection == "desc")
                {
                    
                    query.Sort((a, b) => b.Call_.Caller_.CompareTo(a.Call_.Caller_));
                    
                }
                else if (sortcolumn == "receiver" && columnDirection == "asc")
                {
                    
                    query.Sort((a, b) => a.Call_.Receiver.CompareTo(b.Call_.Receiver));
                    
                }
                
                else if (sortcolumn == "receiver" && columnDirection == "desc")
                {
                    
                    query.Sort((a, b) => b.Call_.Receiver.CompareTo(a.Call_.Receiver));
                    
                }
                
                return query.ToArray();
            }
        }


        //Converts optional sort and filters params to variables
        private static void ConvertParamsToVariables(string sortParams, out string searchString, out string sortDirection, out string[] eventTypefilter)
        {
            var paramArray = sortParams.Split('+');
            searchString = paramArray.Count() > 1 ? paramArray[1] : "";
            sortDirection = paramArray[0];
            eventTypefilter = paramArray.Count() > 2 ? paramArray[2].Split('-') : new string[] { };
        }

        //Sorts the jagged array by the Record Date(Timestamp) in the events using a bubble sort
        private T_Event[][] SortJaggedArrayByDatetime(T_Event[][] t_Events)
        {
            T_Event[] replacementHelper;

            for (int j = 0; j < t_Events.Length - 1; j++)
            {
                for (int i = 0; i < t_Events.Length - 1; i++)
                {
                    int result = DateTime.Compare(t_Events[i][0].Record_Date, t_Events[i + 1][0].Record_Date);
                    if (result < 0)
                    {
                        replacementHelper = t_Events[i + 1];
                        t_Events[i + 1] = t_Events[i];
                        t_Events[i] = replacementHelper;
                    }
                }
            }
            return t_Events;
        }
        #endregion
    }
}
