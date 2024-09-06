using AspireApp.Libraries.Models;
using System.Net.Http.Json;

namespace AspireApp.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class OrchestratorApiClient(HttpClient httpClient)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse> getSourceData(DerivedDataFilter filter, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("/getSourceData", filter);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ActionResponse>(cancellationToken!);
            else
                return new ActionResponse() { Type = response.StatusCode.ToString(), Message = "Not found record" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse> processData(RunImage runImage, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("/processData", runImage);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ActionResponse>(cancellationToken!);
            else
                return new ActionResponse() { Type = response.StatusCode.ToString(), Message = "Not found record" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse> saveRunImage(RunImage record, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("/saveRunImage", record);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ActionResponse>(cancellationToken!);
            else
                return new ActionResponse() { Type = response.StatusCode.ToString(), Message = "Not saved record" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<RunImage> getRunImage(int id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"/getRunImage/{id}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<RunImage>(cancellationToken!);
            else
                return new RunImage();
        }
    }
}
