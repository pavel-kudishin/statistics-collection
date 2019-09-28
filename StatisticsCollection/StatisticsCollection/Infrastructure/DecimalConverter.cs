﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace StatisticsCollection.Infrastructure
{
	public class DecimalConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is decimal)
				return value.ToString();
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (decimal.TryParse(value as string, out decimal dec))
				return dec;
			return value;
		}
	}
}