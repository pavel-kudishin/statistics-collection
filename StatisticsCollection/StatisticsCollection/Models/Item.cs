using StatisticsCollection.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StatisticsCollection.Models
{
	public class Item : INotifyPropertyChanged
	{
		private DateTime _date;
		private string _text;

		public string Id { get; set; }

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

		public string Text
		{
			get => _text;
			set
			{
				if (value == _text)
					return;
				_text = value;
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