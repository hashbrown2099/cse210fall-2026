
//aar0n james 

abstract class MainAccount
{
    public string AccountType { get; protected set; }// public will allow for the  
    public double Balance { get; protected set; }// double allows for decimal values to to be held  balance can be accessed out side of its class with public 

    public MainAccount(string accountType)
    {
        AccountType = accountType;
        Balance = 0.0;// setting the blance to 0
    }

    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            Balance += amount;
            Console.WriteLine($"{amount}$ has been deposited into your account. Your updated balance is now now {Balance}$");
        }
        else
        {
            Console.WriteLine("The amount deposited must be positive");
        }
    }

    public virtual void Withdraw(double amount)
    {
        if (amount > 0 && amount <= Balance)
        {
            Balance -= amount;
            Console.WriteLine($"{amount}$ has been withdrawn from your account, your balance is now : {Balance}$");
        }
        else
        {
            Console.WriteLine("Insufficient FUNDS.... YOU ARE BROKE");
        }
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"{AccountType} Account | Balance : {Balance}$");
    }
}

class SavingsAccount : MainAccount
{
    public SavingsAccount() : base("Savings") { }
}

class CheckingsAccount : MainAccount
{
    public CheckingsAccount() : base("checking") { }

    public override void Withdraw(double amount)
    {
        double overdraftLimit = 0.0;
        if (amount <= Balance + overdraftLimit)
        {
            Balance -= amount;
            Console.WriteLine($"{amount}$ Has been Withdrawn");
        }
        else
        {
            Console.WriteLine($" You do not have an overdraft limit. ");
        }
    }
}

//now im going to make the parent class for the accounts and two types of accounts 

class AccountHolder
{
    public string Username { get; set; } // user name for the account 
    public string PIN { get; set; } // the user can set the PIN
    public Dictionary<String, String> SecurityQuestions { get; set; }// This is the security questions with the answers 
    public List<MainAccount> Accounts { get; set; } 

    public AccountHolder()
    {
        Accounts = new List<MainAccount>();
        SecurityQuestions = new Dictionary<string, string>();
    }

    public bool VerifyPIN(string enteredPIN)
    {
        return enteredPIN == PIN;
    }

    // this is where i built the logic for the sercurity questions 
    public bool VerifySercuirtyQuestions()
    {
        Console.WriteLine("\n Answer the sercurity questions:");
        foreach (var question in SecurityQuestions)
        {
            Console.WriteLine(question.Key);
            Console.Write("Answer: ");
            string answer = Console.ReadLine();
            if (answer.ToLower() != question.Value.ToLower())
            {
                Console.WriteLine(" Incorrect");
                return false;
            }
        }
        return true;
    }
}

class SercuritySoftware
{
    static List<AccountHolder> users = new List<AccountHolder>(); // creates memory for all people ho create account

    static void Main(string[] args)
    {
        Console.WriteLine(" Welcome to Big Money Corperations");
        while (true) // keeps the menus open
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Log In");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option 1,2,3: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                OpenAccount(); // Calling the  method to create an account
            }
            else if (choice == "2")
            {
                LogIn(); 
            }
            else if (choice == "3")
            {
                Console.WriteLine("Goodbye");
                break; 
            }
            else
            {
                Console.WriteLine("Invalid Please enter 1, 2, or 3.");
            }
        }
    }

    static void OpenAccount()
    {
        Console.WriteLine(" Open your account ");
        Console.Write("Create a Username: ");
        string name = Console.ReadLine();

        Console.Write("Set a 4 digit PIN number: ");
        string pin = Console.ReadLine();

        // Fixed security questions sunday 
        var securityQA = new Dictionary<string, string>();
        Console.WriteLine("\nAnswer the following security questions to bypass PIN:");
        string[] questions = {
            "What was the name of your high school?",
            "What city were you born in?",
            "Who is your favorite Prophet from scriptures?"
        };
        foreach (string question in questions)
        {
            Console.Write($"{question} ");
            string answer = Console.ReadLine();
            securityQA[question] = answer;
        }

        // Create the account holder  with savings and checking accounts
        var user = new AccountHolder
        {
            Username = name,
            PIN = pin,
            SecurityQuestions = securityQA,
            Accounts = new List<MainAccount>
            {
                new SavingsAccount(),
                new CheckingsAccount()
            }
        };

        users.Add(user);
        Console.WriteLine(" Account has been created  ");
    }

    static void LogIn()
    {
        Console.WriteLine("Log In");
        Console.Write(" Enter Username:  ");
        string name = Console.ReadLine();

        AccountHolder user = users.Find(u => u.Username.Equals(name, StringComparison.OrdinalIgnoreCase));// when putting into ai it changed look up what this line does before submitting  //https://www.youtube.com/watch?v=nFFKfbuOvQw&t=1s  searches for stored username but disregards case sensaticity

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        int attempts = 0;
        bool loggedIn = false;

        while (attempts < 3 && !loggedIn)
        {
            Console.Write("Enter your 4-digit PIN: ");
            string enteredPIN = Console.ReadLine();

            if (user.VerifyPIN(enteredPIN))
            {
                Console.WriteLine($" Welcome, {user.Username}!");
                loggedIn = true;
            }
            else
            {
                attempts++;
                Console.WriteLine($" Incorrect PIN. Attempts left: {3 - attempts}");
            }
        }

        if (!loggedIn)
        {
            Console.WriteLine("Too many failed attempts. Please answer your security questions.");
            if (user.VerifySercuirtyQuestions())
            {
                Console.WriteLine("Security questions verified. Access granted.");
                loggedIn = true;
            }
            else
            {
                Console.WriteLine(" Access denied.");
            }
        }

        if (loggedIn)
        {
            UserMenu(user); // Show logged-in menu
        }
    }

    static void UserMenu(AccountHolder user)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Account Menu ---");
            Console.WriteLine("1. View Accounts");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Logout");
            Console.Write("Choose an option (1-4): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    foreach (var account in user.Accounts)
                    {
                        account.DisplayInfo();
                    }
                    break;
                case "2":
                    MakeTransaction(user, "deposit");
                    break;
                case "3":
                    MakeTransaction(user, "withdraw");
                    break;
                case "4":
                    Console.WriteLine(" Logging out...");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void MakeTransaction(AccountHolder user, string type)
    {
        Console.WriteLine("\nSelect Account: 1. Savings  2. Checking");
        string accChoice = Console.ReadLine();

        MainAccount selectedAccount = accChoice == "1" ? user.Accounts[0] : user.Accounts[1]; // when editing the program ai changed the if else  this look back at it later before you sumbitt  // https://www.youtube.com/watch?v=dWsjatPhWHQ video for tetrinarary operators 

        Console.Write($"Enter amount to {type}: ");
        double amount;
        if (double.TryParse(Console.ReadLine(), out amount))
        {
            if (type == "deposit")
            {
                selectedAccount.Deposit(amount);
            }
            else
            {
                selectedAccount.Withdraw(amount);
            }
        }
        else
        {
            Console.WriteLine(" Invalid amount.");
        }
    }
}
