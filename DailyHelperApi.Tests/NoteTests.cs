using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyHelper.Entity;
using DailyHelper.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DailyHelperApi.Tests
{
    public class NoteTests
    {
        [Fact]
        public async Task NoteRepository_GetAllNotes_ShouldReturn3Notes()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;

            using (var context= new ApplicationDbContext(options))
            {
                context.Notes.Add(new Note()
                {
                    Id = Guid.NewGuid(),
                    Title = "Test1"
                });
                context.Notes.Add(new Note()
                {
                    Id = Guid.NewGuid(),
                    Title = "Test2"
                });
                context.Notes.Add(new Note()
                {
                    Id = Guid.NewGuid(),
                    Title = "Test3"
                });

                context.SaveChanges();
            }

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new NoteRepository(context);
                IEnumerable<Note> notes = await repository.GetAsync();
                
                //assert
                Assert.Equal(3,notes.Count());
            }
        }
        
        [Fact]
        public async Task NoteRepository_GetNoteById_ShouldReturnNote()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;

            Guid id= Guid.NewGuid();
            using (var context= new ApplicationDbContext(options))
            {
                context.Notes.Add(new Note()
                {
                    Id = id,
                    Title = "Test1"
                });
                context.SaveChanges();
            }
            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new NoteRepository(context);
                Note note = await repository.GetAsync(id);
                //assert
                Assert.Equal(id,note.Id);
            }
        }
        
        [Fact]
        public async Task NoteRepository_PostNote_ThenGetNoteById_ShouldReturnNote()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;

            Guid id= Guid.NewGuid();
            var noteToPut = new Note()
            {
                Id = id,
                Title = "Test1"
            };
           
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new NoteRepository(context);
                await repository.PostAsync(noteToPut);
                Note note = await repository.GetAsync(id);
                
                Assert.Equal(id,note.Id);
            }
        }
        
        [Fact]
        public async Task NoteRepository_RemoveNote_ThenGetNoteById_ShouldReturnNull()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;
            var noteToTest = new Note()
            {
                Id = Guid.NewGuid(),
                Title = "Test1"
            };
            
            using (var context= new ApplicationDbContext(options))
            {
                context.Notes.Add(noteToTest);
                context.SaveChanges();
            }
            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new NoteRepository(context);
                await repository.RemoveAsync(noteToTest);
                var note = await repository.GetAsync(noteToTest.Id);
                //assert
                Assert.Null(note);
            }
        }
        
        [Fact]
        public async Task NoteRepository_PutNote_ThenGetNoteById_ShouldReturnChangedNote()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;
            var id = Guid.NewGuid();
            var noteToTest = new Note()
            {
                Id = id,
                Title = "NewNote"
            };
            
            using (var context= new ApplicationDbContext(options))
            {
                context.Notes.Add(new Note()
                {
                    Id = noteToTest.Id,
                    Title = "OldNote"
                });
                context.SaveChanges();
            }
            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new NoteRepository(context);
                await repository.PutAsync(noteToTest.Id,noteToTest);
                var newNote = await repository.GetAsync(id);
                //assert
                Assert.Equal("NewNote",newNote.Title);
            }
        }
    }
}