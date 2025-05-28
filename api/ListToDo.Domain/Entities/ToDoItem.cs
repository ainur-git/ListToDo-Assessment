using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ListToDo.Core.Entities
{
    public class ToDoItem
    {
        public int ItemToDoId { get; set; }
        public int ListToDoId { get; set; } //Foreign Key to ToDoList
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }

        [JsonIgnore]
        public ToDoList ToDoList { get; set; } // navigation
    }
}
