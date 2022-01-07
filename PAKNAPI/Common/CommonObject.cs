namespace PAKNAPI.Common
{
	public class DropdownObject
	{
		public int Value { get; set; }
		public string Text { get; set; }
		public bool IsMain { get; set; }
	}
	public class DropdownTree
	{
		public int Value { get; set; }
		public string Text { get; set; }
		public int ParentId { get; set; }
		public int UnitLevel { get; set; }
		public bool IsMain { get; set; }
	}
}