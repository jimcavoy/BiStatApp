using BiStatApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class PositionToBoolConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ShootingBout.PositionEnum)value == ShootingBout.PositionEnum.STANDING;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ShootingBout.PositionEnum.STANDING : ShootingBout.PositionEnum.PRONE;
        }
    }
}
