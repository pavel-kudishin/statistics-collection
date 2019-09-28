using StatisticsCollection.Models;

namespace StatisticsCollection.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
		public Item Item { get; set; }

		public ItemDetailViewModel(Item item = null)
		{
			Title = item?.Date.ToString("G");
			Item = item;
		}
	}
}
