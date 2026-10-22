using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        PromptGenerator promptGenerator = new PromptGenerator();
        string choice = "";
        Console.WriteLine(" Happy Halloween What would you like to do in your digital journal today dude?");
        while (choice != "5")
        {
            Console.WriteLine();
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal");
            Console.WriteLine("4. Load the journal");
            Console.WriteLine("5. Quit");
            Console.Write("Choose an option or be cursed: ");
            choice = Console.ReadLine();
            if (choice == "1")
            {
                string prompt = promptGenerator.GetRandomPrompt();
                Console.WriteLine(prompt);
                Console.Write("> ");
                string response = Console.ReadLine();
                Console.Write("How are you feeling today dude? ");
                string mood = Console.ReadLine();
                DateTime currentTime = DateTime.Now;
                string date = currentTime.ToShortDateString();
                Entry entry = new Entry(date, prompt, response, mood);
                journal.AddEntry(entry);
                Console.WriteLine("Entry added. Choose 3 to save it to a folder.");
            }
            else if (choice == "2")
            {
                journal.DisplayAll();
            }
            else if (choice == "3")
            {
                Console.Write("folder to save to: ");
                string filename = Console.ReadLine();
                journal.SaveToFile(filename);
                Console.WriteLine("Journal saved.");
            }
            else if (choice == "4")
            {
                Console.Write("Folder name to load from: ");
                string filename = Console.ReadLine();
                journal.LoadFromFile(filename);
                Console.WriteLine("Journal loaded.");
            }
            else if (choice == "5")
            {
                Console.WriteLine("Goodbye!");
            }
            else
            {
                Console.WriteLine("Please enter a number from 1 to 5. or be doomed!!!");
            }
        }
    }
}
            









class Journal
{
    private List<Entry> entries;
    public Journal()
    {
        entries = new List<Entry>();
    }
    public void AddEntry(Entry entry)
    {
        entries.Add(entry);
    }
    public void DisplayAll()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("Your journal is empty.");
            return;
        }
        foreach (Entry entry in entries)
        {
            entry.Display();
        }
    }
    public void SaveToFile(string filename)
    {
        using (StreamWriter outputFile = new StreamWriter(filename))
        {
            foreach (Entry entry in entries)
            {
                string line = entry.date + "|" + entry.promptText + "|" + entry.entryText + "|" + entry.mood;
                outputFile.WriteLine(line);
            }
        }
    }
    public void LoadFromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        List<Entry> loadedEntries = new List<Entry>();
        foreach (string line in lines)
        {
            string[] parts = line.Split('|');
            if (parts.Length != 4)
            {
                throw new FormatException("Each saved entry must contain four fields.");
            }
            Entry entry = new Entry(parts[0], parts[1], parts[2], parts[3]);
            loadedEntries.Add(entry);
        }
        entries = loadedEntries;
    }
}









class PromptGenerator
{
    private List<string> prompts;
    private Random random;
    public PromptGenerator()
    {
        prompts = new List<string>();
        prompts.Add("Who was the most interesting person I interacted with today?");
        prompts.Add("What was the best part of my day?");
        prompts.Add("How did I see the hand of the Lord in my life today?");
        prompts.Add("What was the strongest emotion I felt today?");
        prompts.Add("If I had one thing I could do over today, what would it be?");
        prompts.Add("What did I accomplish today?");
        prompts.Add("Did I exercise today?");
        prompts.Add("Did I enjoy class today?");
        random = new Random();
    }
    public string GetRandomPrompt()
    {
        int index = random.Next(prompts.Count);
        return prompts[index];
    }
}










class Entry
{
    public string date;
    public string promptText;
    public string entryText;
    public string mood;
    public Entry(string date, string promptText, string entryText, string mood)
    {
        this.date = date;
        this.promptText = promptText;
        this.entryText = entryText;
        this.mood = mood;
    }
    public void Display()
    {
        Console.WriteLine("Date: " + date);
        Console.WriteLine("Vibes: " + mood);
        Console.WriteLine("Prompt: " + promptText);
        Console.WriteLine("Entry: " + entryText);
        Console.WriteLine();
    }
}