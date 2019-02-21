using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace W
{

    public class LocationInfo
    {
        public string Country;
        public string City;
        public override string ToString()
        {
            return $"{City}, {Country}";
        }
    }

    public class WeatherInfo
    {
        public string SkyConditions;
        public string Temperature;
        public override string ToString()
        {
            return $"{SkyConditions}, {Temperature}";
        }
    }

    internal interface IWeatherService{
        Task<IEnumerable<LocationInfo>> GetCities(string country);
        Task<WeatherInfo> GetWeather(LocationInfo location);
    }


    public class WeatherService : IWeatherService
    {
        public WeatherService(string baseUri) {
            _baseUri = new Uri(baseUri);
        }

        private Uri _baseUri;

        private Uri GetApiUri(string methodName)
        {
            return new Uri(_baseUri, methodName);
        }

        #region Serialization
        [XmlRoot("NewDataSet")]
        public class CitiesByCountryDataSet
        {
            [XmlElement("Table")]
            public LocationInfo[] Locations;
        }
        private static XmlSerializer _citiesByCountryDataSetSerializer = new XmlSerializer(typeof(CitiesByCountryDataSet));

        [XmlRoot("NewDataSet")]
        public class WeatherDataSet
        {
            [XmlElement("Weather")]
            public WeatherInfo Weather;
        }
        private static XmlSerializer _weatherDataSetSerializer = new XmlSerializer(typeof(WeatherDataSet));
        #endregion

        public async Task<IEnumerable<LocationInfo>> GetCities(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return null;
            }
            try
            {
                var reader = await GetXmlReader(GetApiUri("GetCitiesByCountry"), new Dictionary<string, string> { { "CountryName", country } });
                var dataset = _citiesByCountryDataSetSerializer.Deserialize(reader) as CitiesByCountryDataSet;
                if (null == dataset.Locations)
                {
                    return null;
                }
                return dataset.Locations;
            }
            catch (Exception ex)
            {
                Debug.Write($"Answer parsing error:\n{ex.Message}", "Parsing");
                return null;
            }
        }

        public async Task<WeatherInfo> GetWeather(LocationInfo location)
        {
            if (null == location)
            {
                return null;
            }
            try
            {
                var reader = await GetXmlReader(GetApiUri("GetWeather"), new Dictionary<string, string> { { "CityName", location.City }, { "CountryName", location.Country } });
                var dataset = _weatherDataSetSerializer.Deserialize(reader) as WeatherDataSet;
                return dataset?.Weather;
            }
            catch (Exception ex)
            {
                Debug.Write($"Answer parsing error:\n{ex.Message}", "Parsing");
                return null;
            }
        }


        private async Task<TextReader> GetXmlReader(Uri url, IDictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            string postData = String.Join("&", parameters.Select(p => string.Format("{0}={1}", Uri.EscapeDataString(p.Key), Uri.EscapeDataString(p.Value))));
            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/xml";
            request.ContentLength = data.Length;
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    await stream.WriteAsync(data, 0, data.Length);
                }
                using (var stream = (await request.GetResponseAsync()).GetResponseStream())
                {
                    var xmlResponse = new XmlDocument();
                    xmlResponse.Load(stream);
                    // This is because we get 'XmlResponse' with text!
                    return new StringReader(xmlResponse.InnerText);
                }
            }
            catch (WebException ex)
            {
                Debug.Write($"Service request error:\n {ex.Message}", "Webservice");
            }
            return null;
        }
    }
}
