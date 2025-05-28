using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListToDo.Application.DTO
{
    public class ItemDto
    {
        public class ToDoItemCreateDto
        {
            public int ListToDoId { get; set; }
            [Required]
            public string Title { get; set; }
            public string Description { get; set; }
            [Required]
            public DateTime? DueDate { get; set; }
            [Required]
            public bool IsCompleted { get; set; }
        }

        public class ToDoItemReadDto
        {
            public int ItemToDoId { get; set; }
            public int ListToDoId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime? DueDate { get; set; }
            public bool IsCompleted { get; set; }
        }
    }
}
