

abstract class Goal
{
   
    private string name;             
    private int points;              
    private GoalStatus status;       
    private DateTime lastCompleted;  

    public string Name => name;
    public int Points => points;
    public GoalStatus Status => status;
    protected void ShowSpinner(string message)
    {
        
    }
}

// daily goals 
// "DailyGoal"  should  inherits from "Goal"
class DailyGoal : Goal
{
    
    public DailyGoal(string name, int points) : base(name, points) { }
    
}

 // this class will manage the goals 
class GoalManager
{
    private List<DailyGoal> dailyGoals = new List<DailyGoal>(); // stores 
    private int totalScore;       // track score
    private int streakCount;      // track streak
    private DateTime lastActiveDate; 

    public GoalManager()
    {

    }
  
  
}

