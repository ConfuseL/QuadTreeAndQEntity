
using UnityEngine;

public enum QuadrantEnum : int
{
    RU=0,
    LU,
    LB,
    RB
}

public class Point
{
    public double x;
    public double y;

    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Rect  {

    public Point o;
    public double Rwidth, Rheight;

    public Rect(double x, double y, double w, double h)
    {
        o = new Point(x, y);
        Rwidth = w / 2;
        Rheight = h / 2;
    }

    public Rect(Point point, double w, double h)
    {
        o = point;
        Rwidth = w / 2;
        Rheight = h / 2;
    }

    public bool IsInclude(Rect target)
    {
        if((Mathf.Abs((float)(target.o.x-o.x))<=(Rwidth-target.Rwidth))&&(Mathf.Abs((float)(target.o.y - o.y)) <= (Rheight - target.Rheight)))
            return true;
        return false;
    }
}