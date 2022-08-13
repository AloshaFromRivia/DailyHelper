using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyHelper.Entity;
using DailyHelper.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DailyHelperApi.Tests
{
    public class TodoTests
    {
        [Fact]
        public async Task TodoRepository_GetAllTodo_ShouldReturn3Todo()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Tasks.Add(new ToDoTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Test1"
                });
                context.Tasks.Add(new ToDoTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Test2"
                });
                context.Tasks.Add(new ToDoTask()
                {
                    Id = Guid.NewGuid(),
                    Title = "Test3"
                });

                 context.SaveChanges();
            }

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TodoRepository(context);
                IEnumerable<ToDoTask> toDoTasks = await repository.GetAsync();

                //assert
                Assert.Equal(3, toDoTasks.Count());
            }
        }

        [Fact]
        public async Task TodoRepository_GetTodoById_ShouldReturnTodo()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;

            Guid id = Guid.NewGuid();
            using (var context = new ApplicationDbContext(options))
            {
                context.Tasks.Add(new ToDoTask()
                {
                    Id = id,
                    Title = "Test1"
                });
                context.SaveChanges();
            }

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TodoRepository(context);
                ToDoTask todo = await repository.GetAsync(id);
                //assert
                Assert.Equal(id, todo.Id);
            }
        }

        [Fact]
        public async Task TodoRepository_PostTodo_ThenGetTodoById_ShouldReturnTodo()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;

            Guid id = Guid.NewGuid();
            var noteToPut = new ToDoTask()
            {
                Id = id,
                Title = "Test1"
            };

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TodoRepository(context);
                await repository.PostAsync(noteToPut);
                ToDoTask todo = await repository.GetAsync(id);
                //assert
                Assert.Equal(id, todo.Id);
            }
        }

        [Fact]
        public async Task TodoRepository_RemoveTodo_ThenGetTodoById_ShouldReturnNull()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;
            var todoToTest = new ToDoTask()
            {
                Id = Guid.NewGuid(),
                Title = "Test1"
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Tasks.Add(todoToTest);
                context.SaveChanges();
            }

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TodoRepository(context);
                await repository.RemoveAsync(todoToTest);
                var todo = await repository.GetAsync(todoToTest.Id);
                //assert
                Assert.Null(todo);
            }
        }

        [Fact]
        public async Task TodoRepository_PutTodo_ThenGetTodoById_ShouldReturnChangedTodo()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DailyDb")
                .Options;
            var id = Guid.NewGuid();
            var todoToTest = new ToDoTask()
            {
                Id = id,
                Title = "NewTodo"
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Tasks.Add(new ToDoTask()
                {
                    Id = todoToTest.Id,
                    Title = "OldTodo"
                });
                context.SaveChanges();
            }

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TodoRepository(context);
                await repository.PutAsync(todoToTest.Id, todoToTest);
                var newTodo = await repository.GetAsync(id);
                //assert
                Assert.Equal("NewTodo", newTodo.Title);
            }
        }
    }
}
    

