using System;

public class sigmoid {

    public static double output(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
    }
	
    public static double derivative (double x)
    {
        return x * (1 - x);
    }
}
