using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.ViewModels.Helpers
{
    public class QueryHelper
    {
        private HttpClient httpClient { get; set; }
        private VssBasicCredential credentials { get; set; }
        private Uri uri { get; set; }

        private string orgName;

        public string OrgName
        {
            get { return orgName; }
            set
            {
                orgName = value;
                uri = new Uri("https://dev.azure.com/" + value);
            }
        }

        private string pat;
        public string PAT
        {
            get { return pat; }
            set
            {
                pat = value;
                credentials = new VssBasicCredential(string.Empty, value);
            }
        }

        public QueryHelper()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> CheckAuthentication()
        {
            string authenticationUri = $"https://dev.azure.com/{OrgName}/_apis/projects?api-version=7.1-preview.1";

            using (HttpClient client = new HttpClient())
            {
                string patToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{pat}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", patToken);

                HttpResponseMessage response = await httpClient.GetAsync(authenticationUri);

                if (response.IsSuccessStatusCode)
                {
                    return "Authentication successful";
                }
                else
                {
                    return "Authentication Failed";
                }
            }
        }
    }
}
