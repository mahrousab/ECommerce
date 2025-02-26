namespace Api.RequestHelper
{
	public class Pagination<T>
	{
		
        public Pagination(int pageIndex, int pageSize, int count,
            IReadOnlyList<T> data)
        {
			PageIndex = pageIndex;
			PageSize = pageSize;
			Count = count;
			Data = data;

		}

		
		public int PageIndex { get; set; } 
		public int PageSize { get; }
		public int Count { get; }
		public IReadOnlyList<T> Data { get; }
	}
}
