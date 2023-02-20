using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using PhoneCentre.Models;

public static class ServiceExtensions
{
    public static IQueryable<T_Event> SortByColumn(this IQueryable<T_Event> query, string columnName, string columnDirection)
    {
        var ascending = columnDirection == "asc";

        return query.Sort(columnName, ascending);
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
            return query.Where(Event => Event.Call_.Caller.ToString().StartsWith(searchString) || Event.Call_.Receiver.ToString()
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

    public static object GetPropertyValue(object source, string propertyName)
    {
        if (source == null) throw new ArgumentException("Value cannot be null.", "source");
        if (propertyName == null) throw new ArgumentException("Value cannot be null.", "propertyName");

        if (propertyName.Contains("."))//complex type nested
        {
            var temp = propertyName.Split(new char[] { '.' }, 2);
            return GetPropertyValue(GetPropertyValue(source, temp[0]), temp[1]);
        }
        else
        {
            var prop = source.GetType().GetProperty(propertyName);
            return prop != null ? prop.GetValue(source, null) : null;
        }
    }
    public static IQueryable<T_Event> Sort(this IQueryable<T_Event> source, string column, bool ascending)
    {
        var type = typeof(Call);
        var property = type.GetProperty(column);

        var direction = ascending ? "OrderBy" : "OrderByDescending";

        ParameterExpression parameter = Expression.Parameter(typeof(T_Event));
        MemberExpression memberExpression = Expression.MakeMemberAccess(Expression.Property(parameter, "Call_"), property);
        LambdaExpression sortExpression = Expression.Lambda(memberExpression, parameter);

        MethodCallExpression orderCall = Expression.Call(typeof(Queryable), direction, new[] { typeof(T_Event), property.PropertyType }, source.Expression, Expression.Quote(sortExpression));

        return source.Provider.CreateQuery<T_Event>(orderCall);
    }

}
