using PhoneCentre.Models;


internal static class EventServiceHelpers
{
    public static IEnumerable<T_Event> SortByColumn(this IEnumerable<T_Event> query, string columnName, string columnDirection)
    {
        var ascending = columnDirection == "asc";

        var fullName = "Call_." + columnName;

        return (ascending ? query.OrderBy(e => GetPropertyValue(e, fullName)) : query.OrderByDescending(e => GetPropertyValue(e, fullName))).AsEnumerable();
    }
    
    public static IEnumerable<T_Event> FilterByEventType(this IEnumerable<T_Event> query, string[] eventTypefilter)
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

    public static IEnumerable<T_Event> FilterBySearch(this IEnumerable<T_Event> query, string searchString)
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

    public static IEnumerable<T_Event> GetPage(this IEnumerable<T_Event> query, int sizeOfPage, int numberOfPagesToSkips)
    {
        return query.Skip((numberOfPagesToSkips - 1) * sizeOfPage)

                    .Take(sizeOfPage)

                    .AsEnumerable();
    }

    public static IEnumerable<T_Event> Apply_Sorting_And_Filtering_To_IQueryable_For_CSV(this IEnumerable<T_Event> query, string sortColumn, string searchString, string sortDirection, string[] eventTypefilter, int chunkSkip, int dataChunkSize)
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
    //Reflection attempts, causes an error

    /*
    public static IEnumerable<T_Event> SortByColumn(this IEnumerable<T_Event> query, string columnName, bool ascending)
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



    //public static IEnumerable<T_Event> SortByColumn(this IEnumerable<T_Event> query, string columnName, string columnDirection)
    //{
    //    var ascending = columnDirection == "asc";
    //    var property = typeof(Call).GetProperty(columnName);
    //    return (ascending ? query.OrderBy(keySelector: Event => property.GetValue(Event.Call_)) : query.OrderByDescending(keySelector: Event => property.GetValue(Event.Call_))).AsEnumerable();
    //}



}
