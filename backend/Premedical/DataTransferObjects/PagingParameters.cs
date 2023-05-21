using System.ComponentModel.DataAnnotations;

namespace Premedical.DataTransferObjects;

public class PagingParameters
{
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 50;

    private int _pageSize = DefaultPageSize;

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
    }

    public string? Search { get; set; }

    public int Skip => (Page - 1) * PageSize;

    public string? NormalizedSearch => string.IsNullOrWhiteSpace(Search) ? null : Search.Trim();
}
