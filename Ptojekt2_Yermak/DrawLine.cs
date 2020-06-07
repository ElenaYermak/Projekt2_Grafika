using System;

public class DrawLine
{
    public Bitmap AlgorytmPrzyrostowy(Bitmap btm, int X1, int Y1, int X2, int Y2)
    {
        int x;
        int y;

        float deltaX, deltaY, m;
        deltaX = X2 - X1;
        deltaY = Y2 - Y1;
        m = deltaY / deltaX;
        float yM = Y1;
        float xM = X1;

        if (Math.Abs(m) >= 1)
        {
            for (y = Y1; y <= Y2; y++)
            {
                btm.SetPixel((int)Math.Floor(xM + 0.5), y, Color.Brown);
                xM = xM + (1 / m);
            }
        }
        else
        {
            for (x = X1; x <= X2; x++)
            {
                btm.SetPixel(x, (int)Math.Floor(yM + 0.5), Color.Brown);
                yM += m;
            }
        }

        if (Y1 > Y2 || X1 > X2)
        {
            if (Math.Abs(m) >= 1)
            {
                xM = X2;
                for (y = Y2; y <= Y1; y++)
                {
                    btm.SetPixel((int)Math.Floor(xM + 0.5), y, Color.Brown);
                    xM = xM + (1 / m);
                }
            }
            else
            {
                yM = Y2;
                for (x = X2; x <= X1; x++)
                {
                    btm.SetPixel(x, (int)Math.Floor(yM + 0.5), Color.Brown);
                    yM += m;
                }
            }
        }

        return btm;
    }
}
