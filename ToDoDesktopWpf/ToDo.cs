using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoDesktopWpf
{
    internal class ToDo
    {
        private int id;
        private string title;
        private bool done;

        public ToDo()
        {
        }

        public ToDo(string title)
        {
            this.title = title;
        }

        public ToDo(int id, string title, bool done)
        {
            this.id = id;
            this.title = title;
            this.done = done;
        }

        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }
        [JsonProperty("title")]
        public string Title { get => title; set => title = value; }
        [JsonProperty("done")]
        public bool Done { get => done; set => done = value; }

        public override string ToString()
        {
            return String.Format("{0,-30} {1}", this.title, this.done);
        }
    }
}
