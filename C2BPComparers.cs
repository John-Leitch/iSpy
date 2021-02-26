using System.Collections;
using System.Drawing;

namespace iSpyApplication
{
    public class HeightComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            return xrect.Height < yrect.Height
                ? 1
                : xrect.Height > yrect.Height ? -1 : xrect.Width < yrect.Width ? 1 : xrect.Width > yrect.Width ? -1 : 0;
        }
    }

    public class WidthComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            return xrect.Width < yrect.Width
                ? 1
                : xrect.Width > yrect.Width ? -1 : xrect.Height < yrect.Height ? 1 : xrect.Height > yrect.Height ? -1 : 0;
        }
    }

    public class AreaComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by area, height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            int xarea = xrect.Width * xrect.Height;
            int yarea = yrect.Width * yrect.Height;
            return xarea < yarea
                ? 1
                : xarea > yarea
                ? -1
                : xrect.Height < yrect.Height
                ? 1
                : xrect.Height > yrect.Height ? -1 : xrect.Width < yrect.Width ? 1 : xrect.Width > yrect.Width ? -1 : 0;
        }
    }

    public class SquarenessComparer : IComparer
    {
        // Return -1, 0, or 1 to indicate whether
        // x belongs before, the same as, or after y.
        // Sort by squareness, area, height, width descending.
        public int Compare(object x, object y)
        {
            Rectangle xrect = (Rectangle)x;
            Rectangle yrect = (Rectangle)y;
            int xsq = System.Math.Abs(xrect.Width - xrect.Height);
            int ysq = System.Math.Abs(yrect.Width - yrect.Height);
            if (xsq < ysq) return -1;
            if (xsq > ysq) return 1;
            int xarea = xrect.Width * xrect.Height;
            int yarea = yrect.Width * yrect.Height;
            return xarea < yarea
                ? 1
                : xarea > yarea
                ? -1
                : xrect.Height < yrect.Height
                ? 1
                : xrect.Height > yrect.Height ? -1 : xrect.Width < yrect.Width ? 1 : xrect.Width > yrect.Width ? -1 : 0;
        }
    }
}