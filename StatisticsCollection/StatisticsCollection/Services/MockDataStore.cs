using StatisticsCollection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsCollection.Services
{
	public class MockDataStore : IDataStore<Item>
	{
		private readonly List<Item> _items;

		public MockDataStore()
		{
			_items =  new List<Item>
			{
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Text = "First item",
					Date = DateTime.Now.AddMonths(-1).AddDays(-2).AddHours(-3)
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Text = "Second item",
					Date = DateTime.Now.AddMonths(-2).AddDays(-2).AddHours(-6)
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Text = "xxx item",
					Date = DateTime.Now.AddMonths(-3).AddDays(-5).AddHours(-9)
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Text = "Fourth item",
					Date = DateTime.Now.AddMonths(-1).AddDays(-21).AddHours(-12)
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Text = "Fifth item",
					Date = DateTime.Now.AddMonths(-10).AddDays(-4).AddHours(-18)
				},
				new Item
				{
					Id = Guid.NewGuid().ToString(),
					Text = "Sixth item",
					Date = DateTime.Now.AddMonths(-21).AddDays(-39).AddHours(-17)
				}
			};
		}

		public async Task<bool> AddItemAsync(Item item)
		{
			_items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			Item oldItem = _items.FirstOrDefault(arg => arg.Id == item.Id);
			_items.Remove(oldItem);
			_items.Add(item);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			Item oldItem = _items.FirstOrDefault(arg => arg.Id == id);
			_items.Remove(oldItem);

			return await Task.FromResult(true);
		}

		public async Task<Item> GetItemAsync(string id)
		{
			return await Task.FromResult(_items.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			return await Task.FromResult(_items.OrderByDescending(i => i.Date));
		}
	}
}