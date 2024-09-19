namespace Twitter.Contract.Models;

public record PagedListResponse<T>
{
    public List<T> Items { get; }

    public PaginationResponse Pagination { get; }

    public PagedListResponse(List<T> items, int page, int pages)
    {
        Items = items;
        Pagination = new(page, pages, page < pages, page > 1);
    }
}

public record PaginationResponse(int Page, int Pages, bool HasNextPage, bool HasPreviousPage);