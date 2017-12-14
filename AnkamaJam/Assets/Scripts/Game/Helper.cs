using System;

public class Helper
{
    private static Random m_random = new Random();

    public static int random(int max)
    {
        return m_random.Next(max);
    }
}
