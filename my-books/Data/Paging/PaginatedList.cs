namespace my_books.Data.Paging
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; } //private по желанию
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            //TotalPages чтобы разместить 8 элементов по 5 на каждой странице (5+3)
            this.AddRange(items);
        }

        //для первых и последних страниц не показывать несуществующие
        public bool HasPreviousPage
        {
            get { return PageIndex > 1; }
        }
        public bool HasNextPage
        {
            get { return PageIndex < TotalPages; }
        }

        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            //например, для 12 элементов по 5 на каждой странице, для отображения страницы 2
            //пропускает (2-1)*5 = 5 элементов сначала, и берет следующие 5
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
