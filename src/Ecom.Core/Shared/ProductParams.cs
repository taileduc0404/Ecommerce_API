namespace Ecom.Core.Shared
{
    public class ProductParams
    {
        //string sort, int? categoryId, int pageNumber, int pageSize

        public int MaxPageSize { get; set; } = 15;
        private int _pageSize = 3;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public string Sort { get; set; }
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;

        private string _search;
        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }
    }
}
