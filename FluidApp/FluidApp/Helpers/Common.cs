using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluidApp.Helpers
{
    public class Common
    {
        public static async Task<MediaFile> LoadCamara(string fileName)
        {
            Location location = new Location();
            location.Latitude = 52.0271723411551;
            location.Longitude = -0.764563267810425;
            location.Altitude = 84;
            location.HorizontalAccuracy = 165;
            location.Speed = -1;
            location.Direction = -1;

            var storePicture = new StoreCameraMediaOptions()
            {
                SaveToAlbum = false,
                Name = $"{fileName}{DateTime.Now.ToString("yyMMddhhmmss")}.jpg",
                AllowCropping = true,
                DefaultCamera = CameraDevice.Rear,
                SaveMetaData = true,
                Location = location
            };
            var photo = await CrossMedia.Current.TakePhotoAsync(storePicture);
            return photo;
        }
    }
}
