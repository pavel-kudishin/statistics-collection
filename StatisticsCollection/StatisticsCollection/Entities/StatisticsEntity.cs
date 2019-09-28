using SQLite;
using System;

namespace StatisticsCollection.Entities
{
	class StatisticsEntity
	{
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public decimal Value { get; set; }
}
}
