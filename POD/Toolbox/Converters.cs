using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace POD.Toolbox
{
    // CORE
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter, IMultiValueConverter
        where T : class, new()
    {

        private static T _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new T();
            }

            return _converter;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    // GENERIC
    public class CollapsedIfTrueOtherwiseVisible : ConverterMarkupExtension<CollapsedIfTrueOtherwiseVisible>
    {
        public CollapsedIfTrueOtherwiseVisible()
        {
        }

        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            bool visibility = (bool)value;
            return !visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility != Visibility.Visible);
        }

    }
    public class CollapsedIfFalseOtherwiseVisible : ConverterMarkupExtension<CollapsedIfFalseOtherwiseVisible>
    {
        public CollapsedIfFalseOtherwiseVisible()
        {
        }

        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            bool visibility = (bool)value;
            return !visibility ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility != Visibility.Collapsed);
        }

    }
    public class CollapsedIfNull : ConverterMarkupExtension<CollapsedIfNull>
    {
        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return (value == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility != Visibility.Collapsed);
        }
    }
    public class CollapsedIfNullOrEmpty : ConverterMarkupExtension<CollapsedIfNullOrEmpty>
    {
        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return (value == null || value.ToString() == "") ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility != Visibility.Collapsed);
        }
    }

    // POD SPECIFIC
    public class ImageBasedOnCardType : ConverterMarkupExtension<ImageBasedOnCardType>
    {
        public ImageBasedOnCardType()
        {

        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string iconName = value switch
            {
                "Book" => "Icon_Book",
                "Electronic" => "Icon_Screen",
                "Figurine" => "Icon_Archer",
                "Firearm" => "Icon_Bullet",
                "Furniture" => "Icon_Furniture",
                "Jewelry" => "Icon_Ring",
                "Media" => "Icon_Media",
                "Tool" => "Icon_Wrench",
                _ => "Icon_Cube"
            };
            return Configuration.AppFramework.FindResource(iconName) as Style;
        }
    }
    

}
