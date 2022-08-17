using System.Collections;

namespace BloodDonation.Results;

public class PagedResult<T> : IEnumerable<T> where T : class
{
    public IList<T> Collection { get; set; } = new List<T>();
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }

    public PaginationInfo Info => new PaginationInfo(TotalCount, Page, Limit);
    public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class PaginationInfo
{
    public long TotalCount { get; }
    public int Page { get; }
    public int Limit { get; }

    public PaginationInfo(long totalCount, int page, int limit)
    {
        TotalCount = totalCount;
        Page = page;
        Limit = limit;
    }
}