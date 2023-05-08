namespace OptimizationMethods; 

public class Pair {
    private double _x;
    private double _y;

    public Pair(double x, double y) {
        _x = x;
        _y = y;
    }

    public double X {
        get => _x;
        set => _x = value;
    }

    public double Y {
        get => _y;
        set => _y = value;
    }

    public static Pair operator *(double c, Pair pair)
        => new Pair(c * pair._x, c * pair._y);
    
    public static Pair operator /(Pair pair, double c)
        => new Pair(pair._x / c, pair._y / c);
    
    public static Pair operator +(Pair a) => a;
    
    public static Pair operator +(Pair a, Pair b) => new Pair(a._x + b._x, a._y + b._y);
    
    public static Pair operator -(Pair a, Pair b) => new Pair(a._x - b._x, a._y - b._y);
    
    public static Pair operator -(Pair pair) => new(-pair._x, -pair._y);

    public override string ToString() {
        return _x + " " + _y;
    }

    public Pair Middle(Pair pair) {
        return  (this + pair) / 2;
    }
}