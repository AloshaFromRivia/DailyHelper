using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace DailyHelper.Client.Models
{
    public class NoteRepository : IRepository<Note>
    {
        private ApiOption _option;
        private RestClient _client;

        public NoteRepository(ApiOption option)
        {
            _option = option;
            _client = new RestClient(option.ApiUrl);
            _client.Authenticator = 
                new JwtAuthenticator("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBbG9zaGEiLCJqdGkiOiI4NTE1NDUxYy03YmRlLTQ3MWUtYWU0ZS1jMjk3YzZlODcwYWQiLCJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJJZCI6IjU5ZWVjMmUyLWY0YmEtNGQyZC04YTkzLWYyZTY0YzI0MWU2OCIsIm5iZiI6MTY2MDMwNDAwMywiZXhwIjoxNjYwMzExMjAzLCJpYXQiOjE2NjAzMDQwMDN9.DOk9YC7czD2Lg6znSP4sH5kwNQgUA5z-BBJd2Fxvzpw");
        }

        public async Task<List<Note>> GetAsync()
        {
            var response = await _client.GetAsync<List<Note>>(CreateRequest("api/note"));
            return response;
        }

        public async Task<Note> GetAsync(Guid id)
        {
            var response = await _client.GetAsync<Note>(CreateRequest($"api/note/{id}"));
            return response;
        }

        public async Task<bool> AddAsync(Note item)
        {
            var response = await _client.PostAsync(CreateRequest<Note>("api/note/",item));
            
            return response.IsSuccessful;
        }

        public async Task RemoveAsync(Guid id)
        {
            var response = await _client.DeleteAsync(CreateRequest($"api/note/{id}"));
        }

        public async Task UpdateAsync(Guid id, Note item)
        {
            var response = await _client.PutAsync<Note>(CreateRequest<Note>($"api/note/{id}",item));
        }

        
        private RestRequest CreateRequest(string resource)
        {
            return new RestRequest(resource)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Accept", "application/json");
        }
        
        private RestRequest CreateRequest<T>(string resource,T body)
        {
            return new RestRequest(resource)
                .AddBody(body)
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Accept", "application/json");
        }
    }
}