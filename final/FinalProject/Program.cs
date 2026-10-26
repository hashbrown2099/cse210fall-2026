// MainAccount.cs
abstract class MainAccount
{
    private string _accountType;
    private decimal _balance;

    public string AccountType
    {
        get { return _accountType; }
    }

    public decimal Balance
    {
        get { return _balance; }
    }

    public MainAccount(string accountType)
    {
        _accountType = accountType;
        _balance = 0m;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("The amount deposited must be positive");
        }
        else if (amount > decimal.MaxValue - _balance)
        {
            Console.WriteLine("That amount is too large.");
        }
        else
        {
            _balance += amount;
            Console.WriteLine($"{amount}$ has been deposited into your account. Your balance is now {Balance}$");
        }
    }


    public virtual void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("The amount withdrawn must be positive");
        }
        else if (amount > _balance)
        {
            Console.WriteLine("Insufficient FUNDS.... YOU ARE BROKE");
        }
        else
        {
            _balance -= amount;
            Console.WriteLine($"{amount}$ has been withdrawn from your account, your balance is now: {Balance}$");
        }
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"{AccountType} Account | Balance : {Balance}$");
    }
}

// SavingsAccount
class SavingsAccount : MainAccount
{
    public SavingsAccount() : base("Savings")
    {
    }
}

// CheckingsAccount
class CheckingsAccount : MainAccount
{
    public CheckingsAccount() : base("Checking")
    {
    }

    public override void Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            Console.WriteLine("You do not have an overdraft limit.");
            return;
        }

        // The base method checks for a positive amount and updates the balance.
        base.Withdraw(amount);
    }
}

// AccountHolder
class AccountHolder
{
    private string _username;
    private string _pin;
    private Dictionary<string, string> _securityQuestions;
    private List<MainAccount> _accounts;

    public string Username
    {
        get { return _username; }
    }

    public AccountHolder(string username, string pin, Dictionary<string, string> securityQuestions)
    {
        _username = username;
        _pin = pin;
        _securityQuestions = new Dictionary<string, string>(securityQuestions);

        _accounts = new List<MainAccount>();
        _accounts.Add(new SavingsAccount());
        _accounts.Add(new CheckingsAccount());
    }

    public bool VerifyPIN(string enteredPIN)
    {
        return enteredPIN == _pin;
    }

    public bool VerifySercuirtyQuestions()
    {
        Console.WriteLine("Answer your security questions:");

        foreach (KeyValuePair<string, string> question in _securityQuestions)
        {
            Console.WriteLine(question.Key);
            Console.Write("Answer: ");
            string answer = (Console.ReadLine() ?? "").Trim();

            if (!string.Equals(answer, question.Value, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Incorrect answer.");
                return false;
            }
        }

        return true;
    }

    public void DisplayAccounts()
    {
        foreach (MainAccount account in _accounts)
        {
            account.DisplayInfo();
        }
    }

    public MainAccount GetAccount(int choice)
    {
        if (choice == 1)
        {
            return _accounts[0];
        }
        else if (choice == 2)
        {
            return _accounts[1];
        }

        return null;
    }
}

// Bank
class Bank
{
    private List<AccountHolder> _users;

    public Bank()
    {
        _users = new List<AccountHolder>();
    }

    public bool AddUser(AccountHolder user)
    {
        if (FindUser(user.Username) != null)
        {
            return false;
        }

        _users.Add(user);
        return true;
    }

    public AccountHolder FindUser(string name)
    {
        foreach (AccountHolder user in _users)
        {
            if (string.Equals(user.Username, name, StringComparison.OrdinalIgnoreCase))
            {
                return user;
            }
        }

        return null;
    }
}

// LoginManager
class LoginManager
{
    private Bank _bank;

    public LoginManager(Bank bank)
    {
        _bank = bank;
    }

    public AccountHolder LogIn()
    {
        Console.WriteLine("Log In");
        Console.Write("Enter Username: ");
        string name = (Console.ReadLine() ?? "").Trim();

        AccountHolder user = _bank.FindUser(name);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return null;
        }

        int attempts = 0;
        bool loggedIn = false;

        while (attempts < 3 && !loggedIn)
        {
            Console.Write("Enter your 4-digit PIN: ");
            string enteredPIN = Console.ReadLine() ?? "";

            if (user.VerifyPIN(enteredPIN))
            {
                Console.WriteLine($"Welcome, {user.Username}!");
                loggedIn = true;
            }
            else
            {
                attempts++;
                Console.WriteLine($"Incorrect PIN. Attempts left: {3 - attempts}");
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
                Console.WriteLine("Access denied.");
            }
        }

        if (loggedIn)
        {
            return user;
        }

        return null;
    }
}

// TransactionManager
class TransactionManager
{
    public void MakeTransaction(AccountHolder user, string type)
    {
        Console.WriteLine("Select Account: 1. Savings  2. Checking");
        string accountChoice = Console.ReadLine() ?? "";
        MainAccount selectedAccount;

        if (accountChoice == "1")
        {
            selectedAccount = user.GetAccount(1);
        }
        else if (accountChoice == "2")
        {
            selectedAccount = user.GetAccount(2);
        }
        else
        {
            Console.WriteLine("Invalid account choice.");
            return;
        }

        Console.Write($"Enter amount to {type}: ");
        decimal amount;

        if (!decimal.TryParse(Console.ReadLine(), out amount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        if (type == "deposit")
        {
            selectedAccount.Deposit(amount);
        }
        else if (type == "withdraw")
        {
            // The actual account type decides which Withdraw method runs.
            selectedAccount.Withdraw(amount);
        }
        else
        {
            Console.WriteLine("Invalid transaction type.");
        }
    }
}

// SercuritySoftware
class SercuritySoftware
{
    private static Bank _bank = new Bank();
    private static LoginManager _loginManager = new LoginManager(_bank);
    private static TransactionManager _transactionManager = new TransactionManager();

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Big Money Corperations");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Log In");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                OpenAccount();
            }
            else if (choice == "2")
            {
                AccountHolder user = _loginManager.LogIn();

                if (user != null)
                {
                    UserMenu(user);
                }
            }
            else if (choice == "3" || choice == null)
            {
                Console.WriteLine("Goodbye");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Enter 1, 2, or 3.");
            }
        }
    }

    static void OpenAccount()
    {
        Console.WriteLine("Open your account ");
        Console.Write("Create a Username: ");
        string name = (Console.ReadLine() ?? "").Trim();

        if (name == "")
        {
            Console.WriteLine("Username cannot be blank.");
            return;
        }

        if (_bank.FindUser(name) != null)
        {
            Console.WriteLine("That username is already taken.");
            return;
        }

        Console.Write("Set a 4 digit PIN number: ");
        string pin = Console.ReadLine() ?? "";
        bool validPIN = pin.Length == 4;

        foreach (char digit in pin)
        {
            if (digit < '0' || digit > '9')
            {
                validPIN = false;
            }
        }

        if (!validPIN)
        {
            Console.WriteLine("PIN must contain exactly 4 digits.");
            return;
        }

        Dictionary<string, string> securityQA = new Dictionary<string, string>();
        string[] questions =
        {
            "What was the name of your high school?",
            "What city were you born in?",
            "Who is your favorite Prophet from scriptures?"
        };

        Console.WriteLine("Answer the following security questions to bypass PIN:");

        foreach (string question in questions)
        {
            Console.WriteLine(question);
            Console.Write("Answer: ");
            string answer = (Console.ReadLine() ?? "").Trim();

            if (answer == "")
            {
                Console.WriteLine("Security answers cannot be blank.");
                return;
            }

            securityQA.Add(question, answer);
        }

        AccountHolder user = new AccountHolder(name, pin, securityQA);

        if (_bank.AddUser(user))
        {
            Console.WriteLine("Account has been created");
        }
        else
        {
            Console.WriteLine("That username is already taken.");
        }
    }

    static void UserMenu(AccountHolder user)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine();
            Console.WriteLine("Account Menu:");
            Console.WriteLine("1. View Accounts");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Logout");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine() ?? "4";

            switch (choice)
            {
                case "1":
                    user.DisplayAccounts();
                    break;
                case "2":
                    _transactionManager.MakeTransaction(user, "deposit");
                    break;
                case "3":
                    _transactionManager.MakeTransaction(user, "withdraw");
                    break;
                case "4":
                    Console.WriteLine("Logging out...");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}