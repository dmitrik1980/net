using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace W
{
    public partial class MainWindow : Window
    {

        private static IWeatherService _weatherService = new WeatherService(Properties.Settings.Default.BaseWeatherServiceUrl);

        public MainWindow()
        {
            InitializeComponent();
            this.cities.IsEnabled = false;
            this.country.Text = Properties.Settings.Default.DefaultCountry;
        }

        private async void getCities_Click(object sender, RoutedEventArgs e)
        {
            var options = await _weatherService.GetCities(this.country.Text);
            if (null == options)
            {
                SetError($"No cities found for '{this.country.Text}'");
            }
            else
            {
                SetSuccess($"Please select one of {options.Count()} place(s) in {this.country.Text}.");
                this.cities.Items.Clear();
                this.cities.Items.Add(new KeyValuePair<LocationInfo, string>(null, "Please select..."));
                foreach (var opt in options)
                {
                    this.cities.Items.Add(new KeyValuePair<LocationInfo, string>(opt, opt.ToString()));
                }
                this.cities.IsEnabled = true;
            }
        }

        private async void cities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (e.Source as ComboBox).SelectedValue;
            if (null != selected)
            {
                var location = ((KeyValuePair<LocationInfo, string>)selected).Key;
                if (null == location)
                {
                    SetError($"Please select a city.");
                    return;
                }
                var answer = await _weatherService.GetWeather(location);
                if (null == answer)
                {
                    SetError($"No data available for {location}");
                }
                else
                {
                    SetSuccess($"The weather today in {location} is {answer}"); // Default with Cloudy/Temperature is unclear, as service always return No Data!
                }
            }
        }

        private void SetSuccess(string message)
        {
            SetMessage(Brushes.LightGreen, message);
        }

        private void SetError(string message)
        {
            SetMessage(Brushes.LightPink, message);
        }

        private void SetMessage(Brush brush, string text)
        {
            this.message.Background = brush;
            this.message.Text = text;
        }
    }
}
