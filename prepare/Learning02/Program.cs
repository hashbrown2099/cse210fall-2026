using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(" what is your first name ");
        String name  = Console.ReadLine();
        Console.WriteLine(" What is your Last Name");
        String LastName = Console.ReadLine();

        Console.WriteLine("Your name is " + LastName  +(",")+ name + "" + LastName);
    }
}