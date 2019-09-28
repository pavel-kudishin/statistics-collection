using SQLite;
using StatisticsCollection.Entities;
using StatisticsCollection.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsCollection.Services
{
	public class DataStore : IDataStore<Item>
	{
		private static async Task<SQLiteAsyncConnection> GetDatabaseConnection()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			SQLiteAsyncConnection database = new SQLiteAsyncConnection(Path.Combine(folderPath, "stats.db3"));
			CreateTableResult createTableResult = await database.CreateTableAsync<StatisticsEntity>();
			return database;
		}

		public async Task<bool> AddItemAsync(Item item)
		{
			SQLiteAsyncConnection connection = await GetDatabaseConnection();
			try
			{
				StatisticsEntity statisticsEntity = new StatisticsEntity()
				{
					Date = item.Date,
					Value = item.Value
				};
				await connection.InsertAsync(statisticsEntity);
			}
			finally
			{
				await connection.CloseAsync();
			}

			return true;
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			SQLiteAsyncConnection connection = await GetDatabaseConnection();
			try
			{
				StatisticsEntity statisticsEntity = await connection.Table<StatisticsEntity>()
					.FirstOrDefaultAsync(i => i.Id == item.Id);

				if (statisticsEntity == null)
				{
					return false;
				}

				statisticsEntity.Date = item.Date;
				statisticsEntity.Value = item.Value;

				await connection.UpdateAsync(statisticsEntity);
			}
			finally
			{
				await connection.CloseAsync();
			}
			return true;
		}

		public async Task<bool> DeleteItemAsync(int id)
		{
			SQLiteAsyncConnection connection = await GetDatabaseConnection();
			try
			{
				StatisticsEntity statisticsEntity = await connection.Table<StatisticsEntity>()
					.FirstOrDefaultAsync(i => i.Id == id);

				if (statisticsEntity == null)
				{
					return false;
				}

				await connection.DeleteAsync(statisticsEntity);
			}
			finally
			{
				await connection.CloseAsync();
			}
			return true;
		}

		public async Task<Item> GetItemAsync(int id)
		{
			SQLiteAsyncConnection connection = await GetDatabaseConnection();
			try
			{
				StatisticsEntity statisticsEntity = await connection.Table<StatisticsEntity>()
					.FirstOrDefaultAsync(i => i.Id == id);

				if (statisticsEntity == null)
				{
					return null;
				}

				Item item = new Item()
				{
					Id = statisticsEntity.Id,
					Date = statisticsEntity.Date,
					Value = statisticsEntity.Value
				};
				return item;
			}
			finally
			{
				await connection.CloseAsync();
			}
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			SQLiteAsyncConnection connection = await GetDatabaseConnection();
			try
			{
				List<StatisticsEntity> statisticsEntities = await connection.Table<StatisticsEntity>()
					.OrderByDescending(i => i.Date)
					.ToListAsync();

				return statisticsEntities.Select(s => new Item()
				{
					Id = s.Id,
					Date = s.Date,
					Value = s.Value
				});
			}
			finally
			{
				await connection.CloseAsync();
			}
		}
	}
}