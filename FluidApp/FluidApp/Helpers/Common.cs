using FluidApp.Entities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FluidApp.Helpers
{
    public class Common
    {
        public static async Task<MediaFile> LoadCamara(string fileName)
        {
            Location location = new Location();
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

        public static bool SendRequest(
                string url, List<ParameterEntity> httpContents, string authorization, HttpMethod httpMethod)
        {
            try
            {
                using (var formData = new MultipartFormDataContent())
                {   
                    var uri = new Uri(url);
                    httpContents.ForEach(httpContent =>
                    {
                        if (string.IsNullOrEmpty(httpContent.Name))
                        {
                            formData.Add(httpContent.httpContent);
                        }
                        else
                        {
                            formData.Add(httpContent.httpContent, httpContent.Name);
                        }
                    });

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = uri,
                        Method = httpMethod,
                        Content = formData
                    };
                    var client = new HttpClient();                    
                    request.Headers.Add("Authorization", authorization);
                    var response = client.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
