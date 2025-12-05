using System.Net.Http;
using System.Text.Json;
using Travel_Requests_App.DTOs.Grades;
namespace Travel_Requests_App.BLL.ClientService
{
    public class HrClientService
    {
        private readonly HttpClient _httpClient;

        public HrClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HRService");
        }



        public async Task<UserResponseDTO> GetEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"api/Users/employee/{id}");

            var user = await response.Content.ReadFromJsonAsync<UserResponseDTO>();
            return user;

        }

        public async Task<UserResponseDTO> UpdateEmployeeByIdAsync(int id, UserRequestDTO userRequestDTO)
        {

            var response = await _httpClient.PutAsJsonAsync($"api/Users/employee/{id}", userRequestDTO);
            response.EnsureSuccessStatusCode();


            var result = await response.Content.ReadFromJsonAsync<UserResponseDTO>();
            return result!;


        }

    }
}
