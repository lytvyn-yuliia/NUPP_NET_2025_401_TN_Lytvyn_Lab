using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journalism.Common
{
    public class Interview : Article
    {
        public string Guest { get; set; }
        public string Location { get; set; }

        //Конструктор класу
        public Interview(string title, string guest, string location)
            : base(title, "Interview")
        {
            Guest = guest;
            Location = location;
        }
    }
}
