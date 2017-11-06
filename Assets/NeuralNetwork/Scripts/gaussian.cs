using System;

// gaussian distribution with mean 0 and variance 1
//Source https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/

public class gaussian {

    public static double NextGaussian()
    {
        double v1, v2, s;
        do
        {
            v1 = 2.0 * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0 * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0 || s == 0);

        s = Math.Sqrt((-2.0 * Math.Log(s)) / s);

        return v1 * s;
    }
}
