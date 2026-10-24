using System;
using System.Collections.Generic;
using System.Threading;


// the menu and completion messages also use a spartan commander theme.

class Program
{
    static void Main(string[] args)
    {
        string choice = "";
        int breathingCount = 0;
        int reflectionCount= 0;
        int listingCount = 0;

        while(choice != "4")
        {
            Console.WriteLine();
              Console.WriteLine("relax big bro");

            Console.WriteLine("completed this run:");
            Console.WriteLine("breathing: " + breathingCount);
            Console.WriteLine("reflection: " + reflectionCount);
            Console.WriteLine("listing: " + listingCount);
            Console.WriteLine("total: " + (breathingCount + reflectionCount + listingCount));
            Console.WriteLine();

            Console.WriteLine("stand ready, spartan. choose your activity.");
            Console.WriteLine("1. breathing activity");
            Console.WriteLine("2. reflection activity");
            Console.WriteLine("3. listing activity");
            Console.WriteLine("4. quit");
            Console.Write("select an option: ");
            choice= Console.ReadLine();

            if(choice == "1")
            {
                BreathingActivity breathing =new BreathingActivity();
                breathing.Run();
                breathingCount++;
            }
          else if(choice == "2")
            {
                ReflectionActivity reflection = new ReflectionActivity();
                reflection.Run();
                reflectionCount++;
            }
            else if (choice == "3")
            {
                ListingActivity listing = new ListingActivity();
                listing.Run();
                listingCount++;
            }
            else if (choice != "4")
            {
                Console.WriteLine("choose a number from 1 to 4, spartan.");
            }
        }

        Console.WriteLine("dismissed, spartan.");
    }
}

public class Activity
{
    private string _names;
    private string _description;
    private int _duration;
    private Random _random = new Random();

    public Activity(string names, string description)
    {
        _names = names;
        _description =description;
    }

    public void DisplayStartingMessage()
    {
        Console.WriteLine();
        Console.WriteLine("welcome to the " + _names + " activity.");
        Console.WriteLine(_description);
        Console.Write("enter the duration in seconds, spartan (suggested: 30): ");

        while (!int.TryParse(Console.ReadLine(), out _duration) || _duration <= 0)
        {
            Console.Write("enter a positive whole number, spartan: ");
        }

        Console.WriteLine("stand ready, spartan.");
        ShowSpinner(4);
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine("well done, spartan!");
        ShowSpinner(3);

        Console.WriteLine(_names + " completed, spartan.");
        Console.WriteLine("you completed " + _duration + " seconds of this activity.");
        ShowSpinner(3);
    }

    public int GetDuration()
    {
        return _duration;
    }

    public void ShowSpinner(int seconds)
    {
        List<string> frames = new List<string> { "|", "/", "-", "\\" };

        for(int i = 0; i < seconds; i++ )
        {
            foreach (string frame in frames)
            {
                Console.Write(frame);
                Thread.Sleep(250);
                Console.Write("\b \b");
            }
        }

        Console.WriteLine();
    }

    public void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i + " ");
            ShowSpinner(1);
        }
    }

    public string GetRandomItem(List<string> items)
    {
        int index = _random.Next(items.Count);
        return items[index];
    }
}

public class BreathingActivity : Activity
{
    public BreathingActivity() : base(
        "breathing",
        "this activity will help you relax by walking you through breathing in and out slowly. " +
        "clear your mind and focus on your breathing.")
    {
    }

    public      void Run( )
    {
        DisplayStartingMessage();
        int secondsLeft = GetDuration();

        while (secondsLeft > 0)
        {
            Console.WriteLine("breathe in...");
            int seconds = Math.Min(4, secondsLeft);
            ShowCountdown(seconds);
            secondsLeft = secondsLeft - seconds ;

            if (secondsLeft > 0)
            {
                Console.WriteLine("breathe out...");
                seconds = Math.Min(4, secondsLeft);
                ShowCountdown(seconds);
                secondsLeft = secondsLeft - seconds ;
            }
        }

       DisplayEndingMessage();
    }
}

public                class ReflectionActivity : Activity
{
    private List<string> _prompts = new List<string>
    {
        "think of a time when you stood up for someone else.",
        "think of a time when you did something really difficult.",
        "think of a time when you helped someone in need.",
        "think of a time when you did something truly selfless."
    };

    private List<string> _questions = new List<string>
    {
        "why was this experience meaningful to you?",
        "have you ever done anything like this before?",
        "how did you get started?",
        "how did you feel when it was complete?",
        "what made this time different than other times when you were not as successful?",
        "what is your favorite thing about this experience?",
        "what could you learn from this experience that applies to other situations?",
        "what did you learn about yourself through this experience?",
        "how can you keep this experience in mind in the future?"
    };

                       public ReflectionActivity() : base(
        "reflection",
        "this activity will help you reflect on times in your life when you have shown strength and resilience. " +
        "this will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
    }

                                        public void Run( )
    {
        DisplayStartingMessage();

        Console.WriteLine("consider this prompt, spartan:");
        Console.WriteLine(GetRandomItem(_prompts));
        Console.WriteLine("press enter when you have an experience in mind.");
        Console.ReadLine();

        int secondsLeft = GetDuration();

        while (secondsLeft > 0)
        {
            Console.WriteLine(GetRandomItem(_questions));
            int seconds = Math.Min(8, secondsLeft);
            ShowSpinner(seconds);
            secondsLeft = secondsLeft - seconds ;
        }

       DisplayEndingMessage();
    }
}

public class ListingActivity : Activity
{
    private List<string> _prompts = new List<string>
    {
        "who are people that you appreciate?",
        "what are personal strengths of yours?",
        "who are people that you have helped this week?",
        "when have you felt the holy ghost this month?",
        "who are some of your personal heroes?"
    };

    public ListingActivity() : base(
        "listing",
        "this activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    public void Run( )
    {
        DisplayStartingMessage();

        Console.WriteLine("list as many responses as you can to this prompt:");
        Console.WriteLine(GetRandomItem(_prompts));
        Console.WriteLine("gather your thoughts, spartan.");
        ShowCountdown(4);

        int count = 0;
        DateTime endTime = DateTime.Now.AddSeconds(GetDuration());

        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string item = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(item))
            {
              count++;
            }
        }

        Console.WriteLine("you listed " + count + " items, spartan.");
       DisplayEndingMessage();
    }
}