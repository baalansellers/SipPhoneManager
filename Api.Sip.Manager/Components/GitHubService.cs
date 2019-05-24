using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Sip.Manager.Models;
using Api.Sip.Manager.Components;
using Newtonsoft.Json;

namespace Api.Sip.Manager.Components
{
    public class GitHubService
    {
        private HttpClient _client { get; }

        public GitHubService(HttpClient client)
        {
            var configFileName = Path.Combine(Directory.GetCurrentDirectory(), "serversettings.json");

            if (!File.Exists(configFileName))
            {
                throw new FileNotFoundException($"Settings file is missing! {configFileName}");
            }

            var settings = JsonConvert.DeserializeObject<ServerSettings>(File.ReadAllText(configFileName));

            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ghb);
            client.DefaultRequestHeaders.Add("User-Agent", "baalansellers");
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache, no-store, must-revalidate, post-check=0, pre-check=0");
            
            _client = client;
        }

        /// <summary>
        /// Gets the full list of files in the root directory of the backup-tftp repository.
        /// </summary>
        public async Task<IEnumerable<ContentModel>> GetContent()
        {
            var response = await _client.GetAsync("/repos/Rev-io/backup-tftp/contents");

            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsAsync<IEnumerable<ContentModel>>();
        }

        /// <summary>
        /// Gets a single file in the root directory of the backup-tftp repository.
        /// </summary>
        public async Task<ContentModel> GetContent(string fileName)
        {
            var response = await _client.GetAsync($"/repos/Rev-io/backup-tftp/contents/{fileName}");

            response.EnsureSuccessStatusCode();
            //TODO: pass 404s through
            return await response.Content.ReadAsAsync<ContentModel>();
        }

        /// <summary>
        /// Find all files that have a name containing the given string.
        /// </summary>
        /// <param name="nameQuery">Can be exact or partial name.</param>
        public async Task<IEnumerable<ContentModel>> SearchContentName(string nameQuery)
        {
            var contents = await GetContent();

            return contents.Where(content => content.name.Contains(nameQuery));
        }

        /// <summary>
        /// Returns the file contents for the given file name as a string.
        /// </summary>
        /// <param name="name">Exact file name</param>
        public async Task<string> GetConfigurationFromContent(string name)
        {
            var response = await _client.GetAsync(new Uri($"https://raw.githubusercontent.com/Rev-io/backup-tftp/master/{name}"));

            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Creates a new file and pushes it directly to GitHub using the branch specified in the given ContentFileModel (i.e. master).
        /// </summary>
        public async Task<string> CreateContent(string fileName, Models.Create.ContentFileModel contentFileModel)
        {
            var response = await _client.PutAsync(
                $"/repos/Rev-io/backup-tftp/contents/{fileName.Replace(" ", "-")}",
                new StringContent(
                    JsonConvert.SerializeObject(contentFileModel),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Updates the contents of a specified file and pushes the change directly to GitHub using the branch specified in the given ContentFileModel (i.e. master).
        /// </summary>
        public async Task<string> UpdateContent(string fileName, Models.Update.ContentFileModel contentFileModel)
        {
            var response = await _client.PutAsync(
                $"/repos/Rev-io/backup-tftp/contents/{fileName.Replace(" ", "-")}",
                new StringContent(
                    JsonConvert.SerializeObject(contentFileModel),
                    System.Text.Encoding.UTF8,
                    "application/json"
                )
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
