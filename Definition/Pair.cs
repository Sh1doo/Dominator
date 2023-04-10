public class Pair
{
    public int a;
    public int b;

    public Pair(int a, int b)
    {
        this.a = a;
        this.b = b;
    }

    public static int CompairPairFirst(Pair pair1,Pair pair2)
    {
        if(pair1.a > pair2.a)
        {
            return 1;
        } 
        else if(pair1.a < pair2.a)
        {
            return -1;
        } 
        else
        {
            return 0;
        }
    }

    public static int CompairPairSecond(Pair pair1, Pair pair2)
    {
        if (pair1.b > pair2.b)
        {
            return 1;
        }
        else if (pair1.b < pair2.b)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

}