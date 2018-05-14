namespace FluidApp
{
    using System;
    using Xamarin.Forms;

    public class CustomDatePicker : DatePicker
    {
        private string _format = null;
        public static readonly BindableProperty NullableDateProperty =
            BindableProperty.Create<CustomDatePicker, DateTime?>(p => p.NullableDate, null);

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        private void UpdateDate()
        {
            if (NullableDate.HasValue)
            {
                if (null != _format)
                    Format = _format;
                Date = NullableDate.Value;
            }
            else
            {
                _format = Format;
                Format = "--/--/--";
                
            }
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Date") NullableDate = Date;
        }
    }
}