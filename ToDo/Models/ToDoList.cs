using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class ToDoList
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public ToDoList(string heading, string description) { Heading = heading; Description = description; }
    }
}