namespace ProductsModule.Models
{
	public class PageList<T>: List<T>
	{
		public int PageIndex { get; set; }
		public int TotalPages { get; set; }

		public PageList(List<T> items, int count, int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			this.AddRange(items);
		}

		public bool HasPreviousPage
		{
			get
			{
				return (PageIndex > 1);
			}
		}

		public bool HasNextPage
		{
			get
			{
				return (PageIndex < TotalPages);
			}
		}

		public static PageList<T> Create (List<T> source, int pageIndex, int pageSize)
		{
			int count = source.Count;
			var items = source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();

			return new PageList<T>(items, count, pageIndex, pageSize);
		}
	}
}
