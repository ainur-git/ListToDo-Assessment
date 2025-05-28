using ListToDo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListToDo.Infrastructure.Data
{
    public class ToDoDbContext:DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options) { } //connext with sql in program.cs to configure

        //tables which is the entities in core/domain
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>()
                .HasKey(t => t.ListToDoId); //primary

            modelBuilder.Entity<ToDoItem>()
                .HasKey(i => i.ItemToDoId); //primary

            modelBuilder.Entity<ToDoList>()
                .HasMany(t => t.Items)
                .WithOne(i => i.ToDoList)
                .HasForeignKey(i => i.ListToDoId); //foreign key to ToDoList
        }
    }
}
