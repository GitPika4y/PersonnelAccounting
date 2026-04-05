namespace Data.Models;

public class PaginationModel<T> where T : EntityModel
{
    public int Count { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyCollection<T> Items { get; set; }

    public IEnumerable<int> Pages
    {
        get
        {
            if (PageSize <= 0)
                return [];

            var pagesCount = (int)Math.Ceiling((double)Count / PageSize);

            const int window = 2;
            var start = Math.Max(1, Page - window);
            var end = Math.Min(pagesCount, Page + window);

            return Enumerable.Range(start, end - start + 1);
        }
    }

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < Pages.Count();
}