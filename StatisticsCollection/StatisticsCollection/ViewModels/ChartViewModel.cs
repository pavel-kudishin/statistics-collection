using Microcharts;
using StatisticsCollection.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsCollection.ViewModels
{
	public class ChartViewModel : BaseViewModel
	{
		public List<Entry> Entries { get; set; }

		public ChartViewModel()
		{
			Title = "График";
		}

		public async Task LoadAsync()
		{
			if (IsBusy)
				return;

			try
			{
				IsBusy = true;

				IEnumerable<Item> items = await DataStore.GetItemsAsync(true);
				List<Item> list = items.ToList();

				Entries = new List<Entry>();
				if (list.Count == 0)
				{
					return;
				}

				DateTime startDate = list[list.Count - 1].Date;
				DateTime endDate = list[0].Date;
				double totalMinutes = (endDate - startDate).TotalMinutes;
				const int INTERVALS = 180;
				double intervalTimeSpan = totalMinutes / INTERVALS;

				for (int i = 0; i < INTERVALS; i++)
				{
					DateTime date = startDate.AddMinutes(intervalTimeSpan * i);
					decimal? value = ItemsViewModel.InterpolateLinear(list, date.AddMinutes(intervalTimeSpan)) -
									ItemsViewModel.InterpolateLinear(list, date);
					if (value != null)
					{
						Entry entry = new Entry((float)value)
						{
							//Label = date.ToString("d")
						};
						Entries.Add(entry);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
