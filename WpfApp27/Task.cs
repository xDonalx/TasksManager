using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp27
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
    }
}