namespace mh4edit;

public class ColorARGB
{
    public byte A { get; set; }
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }

    public ColorARGB(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }
    public ColorARGB(byte r, byte g, byte b)
    {
        A = 255;
        R = r;
        G = g;
        B = b;
    }
}
public static class Colors
{
    public static ColorARGB Black => new ColorARGB(255, 0, 0, 0);
    public static ColorARGB White => new ColorARGB(255, 255, 255, 255);
    public static ColorARGB Red => new ColorARGB(255, 255, 0, 0);
    public static ColorARGB Green => new ColorARGB(255, 0, 255, 0);
    public static ColorARGB Blue => new ColorARGB(255, 0, 0, 255);
    
}