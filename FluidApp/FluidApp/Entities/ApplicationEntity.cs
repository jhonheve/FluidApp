using Newtonsoft.Json;
using System;

namespace FluidApp.Entities
{
    
    public class Applicants
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("created_at")]
        public DateTime? CreatedAt;

        [JsonProperty("href")]
        public string HRef;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("first_name")]
        public string FirstName;

        [JsonProperty("middle_name")]
        public string MiddleName;

        [JsonProperty("last_name")]
        public string LastName;

        [JsonProperty("gender")]
        public string Gender;

        [JsonProperty("dob")]
        public DateTime? DateOfBirth;

        [JsonProperty("telephone")]
        public string Telephone;

        [JsonProperty("mobile")]
        public string Mobile;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("country")]
        public string Country;

    }
}
