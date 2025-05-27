using System.Linq.Expressions;
using DevHabit.API.Contracts.Tags;

namespace DevHabit.API.Mapping;

public static class TagQueries
{
    public static class HabitQueries
    {
        public static Expression<Func<Tag, TagDto>> ProjectToDto()
        {
            return h => new TagDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                CreatedAtUtc = h.CreatedAtUtc,
                UpdatedAtUtc = h.UpdatedAtUtc,
            };
        }

    }

}
