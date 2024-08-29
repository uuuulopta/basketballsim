namespace basketballSim;

public static class LogisticFunc
{
    private static double L = 0.25;
    private static double k = 0.5;
    private static double x0 = -4;

    public static double calculate(double x)
    {
        if (x <= 0) return 0;
        return L / (1 + Math.Pow( Math.E,-k * (x - x0) ) );
    }

}