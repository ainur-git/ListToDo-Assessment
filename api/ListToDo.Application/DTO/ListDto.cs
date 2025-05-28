using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListToDo.Application.DTO.ItemDto;

namespace ListToDo.Application.DTO
{
    public class ListDto
    {
        public class ToDoListCreateDto
        {
            [Required]
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class ToDoListReadDto
        {
            public int ListToDoId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public List<ToDoItemReadDto> Items { get; set; } = new();// to read the item too
        }
    }
}
