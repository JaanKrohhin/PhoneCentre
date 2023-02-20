using CallCentreTask.Data;
using PhoneCentre.Models;
using System.IO.Pipelines;

namespace PhoneCentre.Data.Services
{
    public interface IDbService
    {
        public IQueryable<T_Event> GetData(string searchString, string[] eventTypefilter, string sortColumn, string sortDirection, int numberOfSkips, int size);
        public T_Event[] GetCall(int callId);
        public int?[] GetCallHistory(int caller);
        public async void WriteCSVDataToStream(string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, PipeWriter writer) { }
    }
}
