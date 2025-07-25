using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using ListToDo.Application.Services;
using ListToDo.Application.DTO;
using ListToDo.Core.Entities;


namespace ListToDo.Application.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) 
        {
            config.NewConfig<ItemDto.ToDoItemCreateDto, ToDoItem>();
            config.NewConfig<ToDoItem, ItemDto.ToDoItemReadDto>();

            config.NewConfig<ListDto.ToDoListCreateDto, ToDoList>();
            config.NewConfig<ToDoList, ListDto.ToDoListReadDto>();

        }
    }
}
