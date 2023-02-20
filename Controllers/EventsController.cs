using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PhoneCentre.Models;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Http.Headers;
using System.Web;
using PhoneCentre.Data.Interfaces;

namespace PhoneCentre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IService _eventsService;

        public EventsController(IService service) =>
            _eventsService = service;
        



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
            T_Event[] eventsArray = GetFilteredAndSortedData(sortColumn, searchString, sortDirection, eventTypefilter, pageNumber, rowSize);


            return eventsArray;

        }





        
        /// <summary>
        /// Gets full details of the event by its Call ID
        /// </summary>
        /// <param name="callId">Id of the call to get the details of</param>
        /// <returns>An array of events that all have same callId</returns>
        [HttpGet("details/{callId:int}")]
        public T_Event[] CallDetails(int callId)
        {
                return _eventsService.GetCall(callId);
        }






        /// <summary>
        /// Sorts and filters the data to a file with .csv extension for download
        /// </summary>
        /// <param name="sortColumn">Column to sort by</param>
        /// <param name="sortParams">Parameters that are used for sorting and filtering the data, separated by '+' sign. Direction of the sort: ASC/DESC. The search string to filter by. The events to filter by. A full variable will look like this: "desc+375+EVENT_PICK_UP----"</param>
        /// <returns>A downloadable file with the todays date in the name: "all_records_yyyyMMdd.csv"</returns>
        /// 

        [HttpGet("download/{sortColumn}/{sortParams}")]
        public void DownloadCSV(string sortColumn, string sortParams)
        {
            string searchString, sortDirection;
            string[] eventTypefilter;
            ConvertParamsToVariables(sortParams, out searchString, out sortDirection, out eventTypefilter);


            var filename = $"all_records.csv";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);

            Response.Clear();
            Response.ContentType = "text/csv";

            _eventsService.WriteCSVDataToStream(sortColumn, searchString, sortDirection, eventTypefilter, Response.BodyWriter);

            Response.StartAsync();

        }

        /*
         */




        /// <summary>
        /// Returns the results of all the calls associated with the caller
        /// </summary>
        /// <param name="caller">Number of the caller</param>
        /// <returns>Returns a jagged array of events with the same caller number</returns>
        [HttpGet("history/{caller:int}")]
        public T_Event[][] HistoryDetails(int caller)
        {
            var IdsOfCalls = _eventsService.GetCallHistory(caller);
            T_Event[][] historyOfCalls = new T_Event[IdsOfCalls.Count()][];
            for (int i = 0; i < IdsOfCalls.Count(); i++)
            {
                historyOfCalls[i] = _eventsService.GetCall((int)IdsOfCalls[i]);
            }

            historyOfCalls = SortJaggedArrayByDatetime(historyOfCalls);

            return historyOfCalls;
        }
        #endregion

        #region Methods for sorting and filtering the data to the desired needs


        
        private T_Event[] GetFilteredAndSortedData(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, int pageNumber, int rowSize)
        {
            var eventsArray = _eventsService.GetData(searchString, eventTypefilter, sortColumn, sortDirection, pageNumber, rowSize);

            return eventsArray.ToArray();
        }



        //Converts optional sort and filters params to variables
        private static void ConvertParamsToVariables(string sortParams, out string searchString, out string sortDirection, out string[] eventTypefilter)
        {
            var paramArray = sortParams.Split('+');
            searchString = paramArray.Count() > 1 ? paramArray[1] : "";
            sortDirection = paramArray[0];
            eventTypefilter = paramArray.Count() > 2 ? paramArray[2].Split('-') : new string[] { ""};
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
