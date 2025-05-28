using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListToDo.Core.Entities
{
    /// <summary>
    /// Represents a list of to-do items.
    /// </summary>
    public class ToDoList
    {
        /// <summary>Unique identifier for the to-do list.</summary>
        public int ListToDoId { get; set; }

        /// <summary>Title of the to-do list.</summary>
        public string Title { get; set; }

        /// <summary>Description of the to-do list.</summary>
        public string Description { get; set; }

        /// <summary>Collection of to-do items under this list.</summary>
        public ICollection<ToDoItem> Items { get; set; }
    }
}
