using System;
using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;

class Program
{
    static void Main(string[] args)
    {
          
    Job firstjob = new Job ("Micron" ,"" Engineer", "2014", "2028");

    Job secondjob = new Job ("TESLA", "Flying car lead echnician", "2020", "2029");

    Resume resume = new Resume ("Aaron Hash");

    resume.job.Add(firstjob);
    resume.job.Add(secondjob);

    resume.display_resume ();
    }
}

public class Job
{
    public string company ;
    public string Job_tittle ;
    public string  start_year;
    public string end_year ;

        public Job (string company , string Job_tittle , string start_year , string end_year)
        {
         this.company = company;
        this.Job_tittle = Job_tittle;
        this.start_year = start_year;
        this.end_year = end_year;
    }
    public static void DisplayJob()
    {
        Console.WriteLine("Job_Tttle" +"company"+ "start_year" +"end_year" );
    }
}

public class Resume
{
    string name ;
   public  List<Job> job = new List<Job>();
  public Resume (string name)
   {
     this.name = name;
   }

   public void display_resume ()
   {
     Console.WriteLine($"Name: {name}");
     Console.WriteLine("Jobs:");
        foreach(Job Job in  job)
        {
            Job.DisplayJob();
        }
   }


     
}
  
     
