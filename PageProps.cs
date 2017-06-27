using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class PageProps
    {
        public double PageWidth { get; set; }
        public double PageHeight { get; set; }
        public double MarginTop { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }
        public double MarginBottom { get; set; }

        public PageProps()
        { }

        public PageProps(double PgWidth, double PgHeight, double MgTop, double MgLeft, double MgRight, double MgBottom)
        {
            PageWidth = PgWidth;
            PageHeight = PgHeight;
            MarginTop = MgTop;
            MarginLeft = MgLeft;
            MarginRight = MgRight;
            MarginBottom = MgBottom;
        }
    }
}
