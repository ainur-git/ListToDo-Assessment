using ListToDo.Application.Services;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using static ListToDo.Application.DTO.ItemDto;

namespace LisToDo.Application.Test
{
    public class ToDoItemServiceTest
    {
        private readonly ToDoDbContext _context;
        private readonly ToDoItemServices _service;

        public ToDoItemServiceTest()
        {
            // Set up an in-memory database for testing
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ToDoDbContext(options);
            _service = new ToDoItemServices(_context);
        }

        [Fact]
        public async Task GetAllItemTest()
        {
            var items = new List<ToDoItem>
            {
                new ToDoItem
                {
                    ListToDoId = 1,
                    Title = "Item 1",
                    Description = "This is a test item 1",
                    DueDate = DateTime.Now,
                    IsCompleted = false,
                },
                new ToDoItem
                {
                    ListToDoId = 1,
                    Title = "Item 2",
                    Description = "This is a test item 2",
                    DueDate = DateTime.Now,
                    IsCompleted = true,
                }
            };

            _context.ToDoItems.AddRange(items);
            await _context.SaveChangesAsync();

            var res = await _service.GetAllItemAsync();

            // Assert
            Assert.True(res.Count() >= 2);
        }

        [Fact]
        public async Task GetItemByIdTest()
        {
            var item = new ToDoItem
            {
                ListToDoId = 1,
                Title = "Find 3",
                Description = "This is a test item 3",
                DueDate = DateTime.Now,
                IsCompleted = false,
            };
            
            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();
            
            var res = await _service.GetItemByIdAsync(item.ItemToDoId);
            Assert.Equal("Find 3", res.Title);
        }

        [Fact]
        public async Task CreateItemTest()
        {
            var item = new ToDoItemCreateDto
            {
                ListToDoId = 1,
                Title = "Test item 4",
                Description = "This is a test item 4",
                DueDate = DateTime.Now,
                IsCompleted = false,
            };

            var res = await _service.CreateItemAsync(item);
            Assert.Equal("Test item 4", res.Title);
        }

        [Fact]
        public async Task UpdateItemTest()
        {
            var item = new ToDoItem
            {
                ListToDoId = 1,
                Title = "Before",
                Description = "Test item Before",
                DueDate = DateTime.Now,
                IsCompleted = false,
            };

            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();

            var updateDto = new ToDoItemCreateDto
            {
                ListToDoId = item.ListToDoId, // keep the same list
                Title = "After",
                Description = "Test item After",
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = true,
            };

            var updatedItem = await _service.UpdateItemAsync(item.ItemToDoId, updateDto);


            Assert.True(updatedItem);
            Assert.Equal("After", updateDto.Title);
        }

        [Fact]
        public async Task DeleteItemTest()
        {
            var item = new ToDoItem
            {
                ItemToDoId = 5,
                ListToDoId = 1,
                Title = "Delete 5",
                Description = "This is a test item 5",
                DueDate = DateTime.Now,
                IsCompleted = false,
            };

            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();

            await _service.DeleteItemAsync(item.ItemToDoId);

            var res = await _context.ToDoItems.FindAsync(item.ItemToDoId);

            Assert.Null(res);
        }

        [Fact]
        public async Task CompleteItemTest()
        {
            var item = new ToDoItem
            {
                ItemToDoId = 6,
                ListToDoId = 1,
                Title = "Complete 6",
                Description = "This is a test item 6",
                DueDate = DateTime.Now,
                IsCompleted = false,
            };

            _context.ToDoItems.Add(item);
            await _context.SaveChangesAsync();

            var success = await _service.CompleteAsync(item.ItemToDoId);

            Assert.True(success);
        }


    }
}