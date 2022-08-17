using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DailyHelper.Entity;
using DailyHelper.Models.ViewModels.Requests;
using FluentAssertions;
using Xunit;

namespace DailyHelper.IntegrationTests
{
    public class NoteControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAsync_WithoutAnyNotes_ReturnEmptyResponse()
        {
            //arrange
            await AuthenticateAsync();

            //act
            var response = await TestClient.GetAsync("api/Note");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<List<Note>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnNote_WhenPostExistInTheDatabase()
        {
            //arrange
            await AuthenticateAsync();
            var postResponse = await TestClient.PostAsJsonAsync("api/note/", new NoteRequest()
            {
                Title = "Test Note",
                Description = "Desc for test note"
            });
            var createdPost = await postResponse.Content.ReadFromJsonAsync<Note>();
            //act
            var response = await TestClient.GetAsync($"api/Note/{createdPost.Id}");
            
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedNote = await response.Content.ReadFromJsonAsync<Note>();
            returnedNote.Id.Should().Be(createdPost.Id);
            returnedNote.Title.Should().Be("Test Note");

        }
    }
}