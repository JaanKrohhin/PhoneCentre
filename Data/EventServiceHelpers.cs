using PhoneCentre.Models;

/*
 * "Implement sorting with a custom flexible extension method, without if/else or switch"
 * I have 2 solutions here: 
 *      1. using a dictionary with the column names as the keys, and the actual object to sort by. 
 *      2. using a method for going through the query
 *      
 *  I used the 1st way because 2nd way still required separation using if/else for the columns, which is not ideal. The 1st one still uses an if statement, however it acts more like a check for errors if anything.
 */
internal static class EventServiceHelpers
{
    public static Dictionary<string, Func<T_Event, object>> _sortColumns = new Dictionary<string, Func<T_Event, object>>
    {
        { "Caller", event_ => event_.Call_.Caller_ },
        { "Receiver", event_ => event_.Call_.Receiver },
    };

    //public static IQueryable<T_Event> SortByColumn<T_Event>(this IEnumerable<T_Event> query, Func<T_Event, object> sortBy, bool ascending)
    //{
    //    return ascending ? query.OrderBy(sortBy).AsQueryable() : query.OrderByDescending(sortBy).AsQueryable();
    //}
}
