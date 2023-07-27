using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHW.BLL.Specification.SupportMessagesSpec
{
    public class SupportMessParams
    {
        private const int MaxSize = 50;
        public int PageIndex { get; set; } = 1;
        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxSize ? MaxSize : value; }
        }
        private string search; //By Name

        public string Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }
    }
}
