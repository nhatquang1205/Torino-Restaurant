namespace TorinoRestaurant.Application.Common.Models;

public class ParamsSearchWithMultiSort
    {
        public ParamsSearchWithMultiSort()
        {
            Sort = "ASC";
            SortBy = "Id";
            PageSize = 100;
            CurrentPage = 1;
        }

        public int CurrentPage { set; get; }

        public int PageSize { set; get; }

        public string Sort { set; get; }

        public string SortBy { set; get; }

        [SwaggerExclude]
        public string[] Order
        {
            get
            {
                return String.IsNullOrEmpty(Sort) ? new string[] { "ASC" } : Sort.Split(",");
            }
        }

        [SwaggerExclude]
        public virtual string[] OrderBy
        {
            get
            {
                return String.IsNullOrEmpty(SortBy) ? new string[] { "Id" } : SortBy.Split(",");
            }
        }
    }