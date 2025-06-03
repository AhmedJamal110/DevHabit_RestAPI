namespace DevHabit.API.Contracts.Comman;

public interface ICollectionResponse<T>
{
    List<T> Items { get; set; }
}
