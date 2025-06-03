using System.Linq.Dynamic.Core;
namespace DevHabit.API.Services;

public sealed record SortMapping(string SortFiled , string PropertyName , bool Reverse = false);

public interface ISortMappingDefination;

public sealed class SortMappingDefination<TSource , TDestination> : ISortMappingDefination
{
    public required SortMapping[] Mappings { get; init; }
}

public sealed class SortMapiingProvider(IEnumerable<ISortMappingDefination> sortMappingDefinations)
{
    public SortMapping[] GetMapping<TSource , TDestination>()
    {
        SortMappingDefination<TSource, TDestination>? sortMappingDefination = sortMappingDefinations
            .OfType<SortMappingDefination<TSource, TDestination>>()
            .FirstOrDefault();
    
        if (sortMappingDefination is null)
        {
            throw new InvalidOperationException(
                $"No sort mapping definition found for {typeof(TSource).Name} to {typeof(TDestination).Name}");
        }
    
        return sortMappingDefination.Mappings;

    }
}


internal static class QueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        string? sort,
        SortMapping[] mappings,
        string defaultOrderBy = "id")
    {

        if(string.IsNullOrWhiteSpace(sort))
        {
            return query.OrderBy(defaultOrderBy);
        }

        string[] sortFilds = sort.Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();


        var orderByParts = new List<string>();

        foreach (string item in sortFilds)
        {
            (string SortFiled, bool IsDescending) = ParseSortFiled(item);

            SortMapping sortMapping = mappings
                .First(m => m.SortFiled.Equals(SortFiled, StringComparison.OrdinalIgnoreCase));

            string direction = (IsDescending, sortMapping.Reverse) switch
            {
                (false, false) => "ASC",
                (false, true) => "DESC",
                (true, false) => "DESC",
                (true, true) => "ASC"
            };

            orderByParts.Add($"{sortMapping.PropertyName} {direction}");
        }

        string orderBy = string.Join(", ", orderByParts);

        return query.OrderBy(orderBy);

    }
    
    private static (string SortFiled , bool IsDescending) ParseSortFiled(string filed)
    {
        string[] parts = filed.Split(' ');
        string sortFiled = parts[0];
        bool isDescending = parts.Length > 1 &&
            parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);


        return (sortFiled, isDescending);
    }




}
