using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StatisticsCollection.Models
{
	public class Item : INotifyPropertyChanged
	{
		private DateTime _date;
		private decimal? _value;

		public int Id { get; set; }

		public DateTime Date
		{
			get => _date;
			set
			{
				if (value.Equals(_date))
					return;
				_date = value;
				OnPropertyChanged();
			}
		}

		public TimeSpan Time
		{
			get => Date.TimeOfDay;
			set => Date = Date.Date.Add(value);
		}

		public decimal? Value
		{
			get => _value;
			set
			{
				if (value == _value)
					return;
				_value = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}