using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWpf.Models;

namespace TaskManagerWpf.Services
{
    public class ApiService
    {
        HttpClient client;
        public ApiService(TokenModel token)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5235");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> GetMyReport(Interval period)
        {
            var response = await client.PostAsJsonAsync("report/getmyreport", period);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<KeyValuePair<string, int>>>();
            }
            throw new Exception("Failed to retrieve the report.");
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> MakeBigReport(Interval period)
        {
            var response = await client.PostAsJsonAsync("report/getbigreport", period);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<KeyValuePair<string, int>>>();
            }
            throw new Exception("Failed to retrieve the report.");
        }

        public async Task<bool> AddOccupation(Occupation occupation, UserInfo userInfo)
        {
            var response = await client.PostAsJsonAsync("occupation", new
            {
                ActivityId = occupation.ActivityId,
                EmployeeId = userInfo.Uid,
                Hour = occupation.Hour,
                Minute = occupation.Minute,
                Date = occupation.Date,
                Comment = occupation.Comment,
            });

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            throw new Exception("Failed to add occupation.");
        }

        public async Task<bool> AddActivity(Activity activity)
        {
            if (string.IsNullOrEmpty(activity.Title))
            {
                throw new ArgumentException("Activity title cannot be empty.");
            }

            var response = await client.PostAsJsonAsync("activity", new { Title = activity.Title, Description = activity.Description });

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return false;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            throw new Exception("Failed to add activity.");
        }


        public async Task<IEnumerable<Activity>> GetActivities()
        {
            var response = await client.GetAsync("/activity");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Activity>>();
            }
            throw new Exception("Failed to retrieve activities.");
        }

        public async Task<IEnumerable<Occupation>> GetOccupations()
        {
            var response = await client.GetAsync("/occupation");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Occupation>>();
            }
            throw new Exception("something wrong...");
        }
        public async Task<UserInfo> GetUserInfo()
        {
            var response = await client.GetAsync("auth");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<UserInfo>();
            }
            throw new Exception("something wrong...");
        }

        public async Task<(IEnumerable<Activity> Activities, IEnumerable<Occupation> Occupations)> FetchDataAsync()
        {
            var activities = await GetActivities();
            var occupations = await GetOccupations();

            return (activities, occupations);
        }

    }
}
