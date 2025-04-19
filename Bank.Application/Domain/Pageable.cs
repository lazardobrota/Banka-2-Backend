namespace Bank.Application.Domain;

public class Pageable(int page = 1, int size = 10)
{
    public int Page { get; set; } = page;
    public int Size { get; set; } = size;

    public static Pageable Create(int page = 1, int size = 10)
    {
        return new Pageable(page, size);
    }
}

public class Page<T>
{
    public List<T> Items         { get; set; } = [];
    public int     PageNumber    { get; set; }
    public int     PageSize      { get; set; }
    public int     TotalElements { get; set; }
    public int     TotalPages    => (int)Math.Ceiling((double)TotalElements / PageSize);

    public Page(List<T> items, int pageNumber, int pageSize, int totalElements)
    {
        Items         = items;
        PageNumber    = pageNumber;
        PageSize      = pageSize;
        TotalElements = totalElements;
    }
}
