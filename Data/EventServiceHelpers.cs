using System.Linq.Dynamic.Core;
using PhoneCentre.Models;

public static class EventServiceHelpers
{
    public static IQueryable<T_Event> SortByColumn(this IQueryable<T_Event> query, string columnName, string columnDirection)
    {
        var direction = columnDirection.ToUpper();

        var fullProperty = "Call_." + columnName;

        var fullQueryText = fullProperty + " " + direction;
        return query.OrderBy(fullQueryText);
    }

    //Possible solution
    //https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/advanced?view=aspnetcore-6.0#dynamic-linq
    //https://stackoverflow.com/questions/31955025/generate-ef-orderby-expression-by-string
    public static IQueryable<T_Event> FilterByEventType(this IQueryable<T_Event> query, string[] eventTypefilter)
    {

        if (eventTypefilter.Any(EventType => EventType != ""))
        {

            return query.Where(_event => eventTypefilter.Contains(_event.Event_Type.Event_Id));

        }
        else
        {

            return query;

        }

    }

    public static IQueryable<T_Event> FilterBySearch(this IQueryable<T_Event> query, string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            return query.Where(e => e.Call_.Caller.ToString().StartsWith(searchString) || e.Call_.Receiver.ToString()
                                                                                                      .StartsWith(searchString));
        }
        else
        {
            return query;
        }
    }

    public static IQueryable<T_Event> GetPage(this IQueryable<T_Event> query, int sizeOfPage, int numberOfPagesToSkips)
    {
        return query.Skip((numberOfPagesToSkips - 1) * sizeOfPage)

                    .Take(sizeOfPage)

                    ;


    }

    public static IQueryable<T_Event> Apply_Sorting_And_Filtering_To_IQueryable_For_CSV(this IQueryable<T_Event> query, string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, int chunkSkip, int dataChunkSize)
    {

        return query.SortByColumn(sortColumn, sortDirection)

            .FilterBySearch(searchString)

            .FilterByEventType(eventTypefilter)

            .GetPage(dataChunkSize, chunkSkip);

    }

    public static object GetPropertyValue(object src, string propName)
    {
        if (src == null) throw new ArgumentException("Value cannot be null.", "src");
        if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

        if (propName.Contains("."))//complex type nested
        {
            var temp = propName.Split(new char[] { '.' }, 2);
            return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
        }
        else
        {
            var prop = src.GetType().GetProperty(propName);
            return prop != null ? prop.GetValue(src, null) : null;
        }
    }

}
