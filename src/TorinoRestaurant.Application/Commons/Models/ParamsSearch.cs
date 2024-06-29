namespace TorinoRestaurant.Application.Common.Models;

public record ParamsSearch
{
    public ParamsSearch()
    {
        Sort = "ASC";
        SortBy = "Id";
        Keyword = "";
        PageSize = 100;
        CurrentPage = 1;
    }

    public string? Keyword { get; set; }

    [SwaggerExclude]
    public string? KeySearch { 
        get {
            return Keyword?.Trim().ToLower();
        }
     }

    public int CurrentPage { set; get; }

    public int PageSize { set; get; }

    public string Sort { set; get; }

    public string SortBy { set; get; }

    [SwaggerExclude]
    public string Order
    {
        get
        {
            return String.IsNullOrEmpty(Sort) ? "ASC" : Sort;
        }
    }

    [SwaggerExclude]
    public virtual string OrderBy
    {
        get
        {
            return String.IsNullOrEmpty(SortBy) ? "Id" : SortBy;
        }
    }
}