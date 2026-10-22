using System;
using System.Dynamic;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        Fraction fraction1 = new Fraction();
        Fraction fraction2 = new Fraction(7);
        Fraction fraction 3 = new Fraction(4,5);

         Console.WriteLine(fraction1.GetFractionString());
        Console.WriteLine(fraction2.GetFractionString());
        Console.WriteLine(fraction3.GetFractionString());
    }
}

class Fraction
{
  private int _top;
  private int _bottom;


        public Fraction()
    {
        _top = 1 ;
        _bottom = 1;
    }

        public  Fraction (int number)
    {
        _top = number ;
        _bottom = 1 ;
    }

        public  Fraction (int top, int bottom)
    {
        _top = top;
        _bottom = bottom;
    }

    public int GetTop()
    {
        return  _top; 
    }
    public void SetTop(int top)
{
    _top = top;
}

public int GetBottom()
{
    return _bottom;
}

public void SetBottom(int bottom)
{
    _bottom = bottom;
}
        
    public string GetFractionString()
    {
        return (_top + "/" + _bottom);
    }
    public double GetDecimalValue()
    {
        return (double) (_top / _bottom );
    }
}