using ListToDo.Application.Services;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using static ListToDo.Application.DTO.ListDto;

namespace LisToDo.Application.Test
{
    public class ToDoListServiceTest
    {
        private readonly ToDoDbContext _context;
        private readonly ToDoListServices _service;

        public ToDoListServiceTest()
        {
            // Set up an in-memory database for testing
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ToDoDbContext(options);
            _service = new ToDoListServices(_context);
        }

        [Fact]
        public async Task GetAllItemTest()
        {
            var items = new List<ToDoList>
            {
                new ToDoList
                {
                    Title = "Task 1",
                    Description = "This is a test task 1",
                },
                new ToDoList
                {
                    Title = "Task 2",
                    Description = "This is a test task 2",
                }
            };

            _context.ToDoLists.AddRange(items);
            await _context.SaveChangesAsync();

            var res = await _service.GetAllItemAsync();

            // Assert
            Assert.True(res.Count() >= 2);
        }

        [Fact]
        public async Task GetListByIdTest()
        {
            var item = new ToDoList
            {
                ListToDoId = 3,
                Title = "Find 3",
                Description = "This is a test task 3",
            };

            _context.ToDoLists.Add(item);
            await _context.SaveChangesAsync();

            var res = await _service.GetItemByIdAsync(item.ListToDoId);
            Assert.Equal("Find 3", res.Title);
        }

        [Fact]
        public async Task CreateTaskTest()
        {
            var item = new ToDoListCreateDto
            {
                Title = "Test task 4",
                Description = "This is a test task 4",
            };

            var res = await _service.CreateItemAsync(item);
            Assert.Equal("Test task 4", res.Title);
        }

        [Fact]
        public async Task UpdateTaskTest()
        {
            var item = new ToDoList
            {
                ListToDoId = 5,
                Title = "Before",
                Description = "Test task Before",
            };

            _context.ToDoLists.Add(item);
            await _context.SaveChangesAsync();

            var updateDto = new ToDoListCreateDto
            {
                Title = "After",
                Description = "Test task After",
            };

            var updatedItem = await _service.UpdateItemAsync(item.ListToDoId, updateDto);


            Assert.True(updatedItem);
            Assert.Equal("After", updateDto.Title);
        }

        [Fact]
        public async Task DeleteTaskTest()
        {
            var item = new ToDoList
            {
                Title = "Delete 6",
                Description = "This is a test item 6",
            };

            _context.ToDoLists.Add(item);
            await _context.SaveChangesAsync();

            await _service.DeleteItemAsync(item.ListToDoId);

            var res = await _context.ToDoItems.FindAsync(item.ListToDoId);

            Assert.Null(res);
        }
    }
}