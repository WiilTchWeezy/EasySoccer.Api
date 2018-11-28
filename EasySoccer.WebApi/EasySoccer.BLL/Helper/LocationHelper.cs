using System;

namespace EasySoccer.BLL.Helper
{
    public class LocationHelper
    {
        public static double Haversine(double lat1, double lat2, double lon1, double lon2)
        {
            const double r = 6371;

            var sdlat = Math.Sin((lat2 - lat1) / 2);
            var sdlon = Math.Sin((lon2 - lon1) / 2);
            var q = sdlat * sdlat + Math.Cos(lat1) * Math.Cos(lat2) * sdlon * sdlon;
            var d = 2 * r * Math.Asin(Math.Sqrt(q));

            return d;
        }
    }
}
