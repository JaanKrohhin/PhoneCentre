using PhoneCentre.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/*
 * "Implement sorting with a custom flexible extension method, without if/else or switch"
 * 
 *  Using a dictionary with the column names as the keys, and the actual object to sort by. 
 *      
 *  
 */
internal static class EventServiceHelpers
{
    public static Dictionary<string, Func<T_Event, object>> _sortColumns = new Dictionary<string, Func<T_Event, object>>
    {
        { "Caller", event_ => event_.Call_.Caller },
        { "Receiver", event_ => event_.Call_.Receiver },
    };


    public static IQueryable<T_Event> SortByColumn(this IQueryable<T_Event> query, string columnName, string columnDirection)
    {
        var ascending = columnDirection == "asc";

        return (ascending ? query.OrderBy(_sortColumns[columnName]) : query.OrderByDescending(_sortColumns[columnName])).AsQueryable();
    }
    
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

                    .AsQueryable();
    }



    //Reflection attempts, causes an error

    /*
    public static IQueryable<T_Event> SortByColumn(this IQueryable<T_Event> query, string columnName, bool ascending)
    {
        PropertyInfo property = typeof(Call).GetProperty(columnName);
        if (property == null)
        {
            throw new ArgumentException($"Column {columnName} not found on type {typeof(Call).Name}");
        }

        ParameterExpression parameter = Expression.Parameter(typeof(T_Event));
        MemberExpression propertyAccess = Expression.MakeMemberAccess(Expression.Property(parameter, "Call_"), property);
        LambdaExpression sortExpression = Expression.Lambda(propertyAccess, parameter);

        MethodCallExpression orderByCall = Expression.Call(
            typeof(Queryable),
            ascending ? "OrderBy" : "OrderByDescending",
            new[] { typeof(T_Event), property.PropertyType },
            query.Expression,
            Expression.Quote(sortExpression));

        return query.Provider.CreateQuery<T_Event>(orderByCall);
    }*/



    //public static IQueryable<T_Event> SortByColumn(this IQueryable<T_Event> query, string columnName, string columnDirection)
    //{
    //    var ascending = columnDirection == "asc";
    //    var property = typeof(Call).GetProperty(columnName);
    //    return (ascending ? query.OrderBy(keySelector: Event => property.GetValue(Event.Call_)) : query.OrderByDescending(keySelector: Event => property.GetValue(Event.Call_))).AsQueryable();
    //}



}
