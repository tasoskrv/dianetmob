using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dianet.Model
{
    public class SearchRecord
    {
        public string DisplayName { get; set; }
        public int Id { get; set; }
        public SearchRecord()
        { }
        public SearchRecord(string Name) {
            this.DisplayName = Name;
        }
    }
}
