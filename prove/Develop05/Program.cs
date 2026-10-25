
class Program
{
    // Creativity: One Punch Man hero levels increase every 500 points.
    // The program shows points to the next level and announces level increases.
    static void Main(string[] args)
    {
        GoalManager manager = new GoalManager ();
        string choice ="";
        while (choice != "6" && choice != null)
        {
            Console.WriteLine();
            Console.WriteLine("Eternal Quest: Train to Be One Punch Man");
            manager.DisplayScore();
            Console.WriteLine("Simple goals: 100 points once.");
            Console.WriteLine("Eternal goals: 25 points every time you record them.");
            Console.WriteLine("Checklist goals: 50 points each time + 200 once when finished.");
            Console.WriteLine("Every 500 total points raises your hero level.");
            Console.WriteLine("1. Create Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Save Progress");
            Console.WriteLine("4. Load Progress");
            Console.WriteLine("5. Record Event");
            Console.WriteLine("6. Quit");
            Console.Write("Pick an option: " );
            choice = Console.ReadLine();
            if (choice == "1")
            {
                manager.CreateGoal();
            }
            else if (choice =="2")
            {
                manager.DisplayGoals();
            }
            else if (choice == "3")
            {
                manager.SaveProgress();
            }
            else if (choice == "4")
            {
                manager.LoadProgress();
            }
            else if (choice == "5")
            {
                manager.RecordEvent();
            }
            else if (choice != "6" && choice != null)
            {
                Console.WriteLine("Enter a number from 1 to 6.");
            }
        }
        Console.WriteLine("Keep crushing those goals!");
    }
}
abstract class Goal
{
    private string name;
    private string description;
    private int points;
    public Goal(string name, string description, int points)
    {
        this.name = name;
        this.description = description;
        this.points = points;
    }
    public virtual int RecordEvent()
    {
        return points;
    }
    public virtual bool IsComplete()
    {
        return false;
    }
    public virtual void Display()
    {
        string check = "[ ]";
        if (IsComplete())
        {
            check = "[X]";
        }
        Console.WriteLine(check + " " + name + " (" + description + ") -- " + points + " points");
    }
    public virtual string GetStringRepresentation()
    {
        return name + "|" + description + "|" + points;
    }
}
class SimpleGoal : Goal
{
    private bool completed;
    public SimpleGoal(string name, string description, bool completed)
        : base(name, description, 100)
    {
        this.completed = completed;
    }
    public override int RecordEvent()
    {
        if (completed == true)
        {
            Console.WriteLine("This goal is already complete.");
            return 0;
        }
        completed = true;
        return base.RecordEvent();
    }
    public override bool IsComplete()
    {
        return completed;
    }
    public override string GetStringRepresentation()
    {
        return "SimpleGoal|" + base.GetStringRepresentation() + "|" + completed;
    }
}
class EternalGoal : Goal
{
    public EternalGoal(string name, string description)
        : base(name, description, 25)
    {
    }
    public override string GetStringRepresentation()
    {
        return "EternalGoal|" + base.GetStringRepresentation();
    }
}
class ChecklistGoal : Goal
{
    private int target;
    private int bonus = 200;
    private int amountCompleted;
    public ChecklistGoal(string name, string description, int target, int amountCompleted)
        : base(name, description, 50)
    {
        this.target = target;
        this.amountCompleted = amountCompleted;
    }
    public override int RecordEvent()
    {
        if (IsComplete())
        {
            Console.WriteLine("This checklist is already complete.");
            return 0;
        }
        amountCompleted = amountCompleted + 1;
        if (IsComplete())
        {
            Console.WriteLine("Checklist complete! Bonus: " + bonus + " points.");
            return base.RecordEvent() + bonus;
        }
        return base.RecordEvent();
    }
    public override bool IsComplete()
    {
        if (amountCompleted >= target)
        {
            return true;
        }
        return false;
    }
    public override void Display()
    {
        base.Display();
        Console.WriteLine("   Completed " + amountCompleted + "/" + target + " times | Finish bonus: " + bonus + " points");
    }
    public override string GetStringRepresentation()
    {
        return "ChecklistGoal|" + base.GetStringRepresentation()
            + "|" + target + "|" + bonus + "|" + amountCompleted;
    }
}
class GoalManager
{
    private List<Goal> goals = new List<Goal>();
    private int totalScore= 0;
    public void DisplayScore()
    {
        int level = totalScore / 500 + 1;
        int pointsneeded = level * 500 - totalScore;
        Console.WriteLine("Score: " + totalScore + " | Hero Level: " + level);
        Console.WriteLine("Points to next level: " + pointsneeded);
    }
    public void CreateGoal()
    {
        Console.WriteLine("1. Simple Goal: 100 points");
        Console.WriteLine("2. Eternal Goal: 25 points each time");
        Console.WriteLine("3. Checklist Goal: 50 points each time + 200 finishing bonus");
        int type = ReadNumber("Choose a goal type: ", 1);
        if (type > 3)
        {
            Console.WriteLine("Choose a number from 1 to 3.");
            return;
        }
        string name = ReadText("Goal name: ");
        string description = ReadText("Description: ");
        Goal goal;
        if (type == 1)
        {
            goal = new SimpleGoal(name, description, false);
        }
        else if (type == 2)
        {
            goal = new EternalGoal(name, description);
        }
        else
        {
            int target = ReadNumber("How many times must you do this? ", 1);
            goal = new ChecklistGoal(name, description, target, 0);
        }
        goals.Add(goal);
        Console.WriteLine ("Goal created!");
    }
    public void DisplayGoals()
    {
        if (goals.Count == 0)
        {
            Console.WriteLine("You do not have any goals yet.");
        }
        for (int i = 0; i < goals.Count; i++)
        {
            Console.Write((i + 1) + ". ");
            goals[i].Display();
        }
    }
    public void RecordEvent()
    {
        DisplayGoals();
        if (goals.Count == 0)
        {
            return;
        }
        int choice = ReadNumber("Which goal did you complete? ", 1);
        if (choice > goals.Count)
        {
            Console.WriteLine("That goal number does not exist.");
            return;
        }
        int oldLevel = totalScore / 500 + 1;
        int earned = goals[choice-1].RecordEvent();
        totalScore = totalScore + earned;
        Console.WriteLine("You earned " + earned + " points!");
        int newLevel = totalScore / 500 + 1;
        if (newLevel > oldLevel)
        {
            Console.WriteLine("LEVEL UP! You are now Hero Level " + newLevel + "!");
        }
    }
    public void SaveProgress()
    {
        using (StreamWriter writer = new StreamWriter("progress.txt"))
        {
            writer.WriteLine(totalScore);
            foreach (Goal goal in goals)
            {
                writer.WriteLine(goal.GetStringRepresentation());
            }
        }
        Console.WriteLine("Progress saved to progress.txt!");
    }
    public void LoadProgress()
    {
        if (!File.Exists("progress.txt"))
        {
            Console.WriteLine("No saved progress found.");
            return;
        }
        string[] lines = File.ReadAllLines("progress.txt");
        int savedScore = int.Parse(lines[0]);
        List<Goal> loadedGoals = new List<Goal>();
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('|');
            string name = parts[1];
            string description = parts[2];
            if (parts[0] == "SimpleGoal")
            {
                bool done = bool.Parse(parts[4]);
                SimpleGoal simplegoal = new SimpleGoal(name, description, done);
                loadedGoals.Add(simplegoal);
            }
            else if (parts[0] == "EternalGoal")
            {
                EternalGoal eternalGoal = new EternalGoal(name, description);
                loadedGoals.Add(eternalGoal);
            }
            else if (parts[0] == "ChecklistGoal")
            {
                int target = int.Parse(parts[4]);
                int done = int.Parse(parts[6]);
                ChecklistGoal checklistgoal = new ChecklistGoal(name, description, target, done);
                loadedGoals.Add(checklistgoal);
            }
            else
            {
                Console.WriteLine("The saved goal type is not recognized.");
                return;
            }
        }
        goals = loadedGoals;
        totalScore = savedScore;
        Console.WriteLine("Progress loaded!");
    }
    private int ReadNumber(string prompt, int minimum)
    {
        int number;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out number) || number < minimum)
        {
            Console.Write("Enter a whole number of at least " + minimum + ": ");
        }
        return number;
    }
    private string ReadText(string prompt)
    {
        Console.Write(prompt);
        string text = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(text) || text.Contains("|"))
        {
            Console.Write("Enter some text without the | character: ");
            text = Console.ReadLine();
        }
        return text;
    }
}
