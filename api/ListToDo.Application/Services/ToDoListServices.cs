using ListToDo.Application.Interfaces;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListToDo.Application.DTO.ItemDto;
using static ListToDo.Application.DTO.ListDto;

namespace ListToDo.Application.Services
{
    public class ToDoListServices : IToDoListServices 
    {
        private readonly ToDoDbContext _context;
        public ToDoListServices(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToDoListReadDto>> GetAllItemAsync()
        {
            var lists = await _context.ToDoLists
                .Include(l => l.Items)
                .ToListAsync();

            return lists.Adapt<List<ToDoListReadDto>>();
        }
                                                                                    
        public async Task<ToDoListReadDto?> GetItemByIdAsync(int id)
        {
            var list = await _context.ToDoLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.ListToDoId == id);

            return list.Adapt<ToDoListReadDto>();
        }

        public async Task<IEnumerable<ToDoListReadDto>> GetItemByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Enumerable.Empty<ToDoListReadDto>();

            var res = await _context.ToDoLists
                .Include(l => l.Items) // Ensure nested items are loaded
                .Where(l => EF.Functions.Like(l.Title.ToLower(), $"%{title.ToLower()}%")) // More efficient & SQL-compatible
                .ToListAsync();

            return res.Adapt<List<ToDoListReadDto>>();
        }

        public async Task<ToDoListReadDto> CreateItemAsync(ToDoListCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            var entity = dto.Adapt<ToDoList>();
            _context.ToDoLists.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<ToDoListReadDto>();
        }

        public async Task<bool> UpdateItemAsync(int id, ToDoListCreateDto dto)
        {
            var entity = await _context.ToDoLists.FindAsync(id);
            if (entity == null) return false;

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            dto.Adapt<ToDoList>();

            _context.ToDoLists.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var entity = await _context.ToDoLists.FindAsync(id);
            if (entity == null) return false;

            _context.ToDoLists.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
