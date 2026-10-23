using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        // Extra feature: Typing hint shows the first letter of each hidden word.
        // Hints last until the next Enter press and do not hide additional words.

        Reference reference = new Reference("Alma", 37, 6, 7);

        string text = "Now ye may suppose that this is foolishness in me; but behold I say unto you, "+ "that by small and simple things are great things brought to pass; " + "and small means in many instances doth confound the wise. " + "And the Lord God doth work by means to bring about his great and eternal purposes; "   + "and by very small means the Lord doth confound the wise " + "and bringeth about the salvation of many souls.";
        Scripture scripture = new Scripture(reference, text);

        bool running = true;
        bool showHint = false;
        while (running)
        {
            Console.Clear();
            if (showHint == true)
            {
                Console.WriteLine(scripture.GetHintText());
            }
            else
            {
                Console.WriteLine(scripture.GetDisplayText());
            }

            Console.WriteLine();

            if (scripture.iscompletleyhiddden())
            {
                running = false;
            }
            else
            {
                Console.WriteLine("Press Enter to hide words, type hint for help, or quit to stop.");
                string input = Console.ReadLine();

                if (input == "quit" || input == null)
                {
                    running = false;
                }
                else if (input == "hint")
                {
                    showHint = true;
                }
                else if (input == "")
                {
                    showHint = false;
                    scripture.HideWords(3);
                }
            }
        }
    }
}
class Scripture
{
    private Reference _reference;
    private Random _random;
    private List<Word> _words;
    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();
        _random = new Random();

        string[] wordTexts = text.Split(' ');

        foreach (string wordText in wordTexts)
        {
            Word word = new Word(wordText);
            _words.Add(word);
        }
    }

    public void HideWords(int numbertohide)
    {
        int count = 0;

        while (count < numbertohide)
        {
            int index = _random.Next(_words.Count);
            _words[index].hide();
            count++;
        }
    }
    public string GetDisplayText()
    {
        string displayText = _reference.GetDisplayText();

        foreach (Word word in _words)
        {
            displayText = displayText + " " + word.GetDisplayText();
        }

        return displayText;
    }
    public string GetHintText()
    {
        string displayText = _reference.GetDisplayText();

        foreach (Word word in _words)
        {
            displayText = displayText + " " + word.GetHintText();
        }

        return displayText;
    }
    public bool iscompletleyhiddden()
    {
        foreach (Word word in _words)
        {
            if (word.ishidden() == false)
            {
                return false;
            }
        }
        return true;
    }
}
class Word
{
    private string _word;
    private bool hidden;

    public Word(string word)
    {
        _word = word;
        hidden = false;
    }
    public void hide()
    {
        hidden = true;
    }
    public bool ishidden()
    {
        return hidden;
    }
    public String GetDisplayText()
    {
        if (hidden == true)
        {
            string hiddenText = "";
            int count = 0;

            while (count < _word.Length)
            {
                hiddenText = hiddenText + "_";
                count++;
            }

            return hiddenText;
        }
        else
        {
            return _word;
        }
    }

    public String GetHintText()
    {
        if (hidden == true)
        {
            string hintText = "";
            int count = 0;

            while (count < _word.Length)
            {
                if (count == 0)
                {
                    hintText = hintText + _word[count];
                }
                else
                {
                    hintText = hintText + "_";
                }
                count++;
            }
            return hintText;
        }
        else
        {
            return _word;
        }
    }
}

class Reference
{
    private string book;
    private int chapter;
    private int verse;
    private int endverse;
    public Reference(string book, int chapter, int verse)
    {
        this.book = book;
        this.chapter = chapter;
        this.verse = verse;
        this.endverse = verse;
    }
    public Reference(string book, int chapter, int verse, int endverse)
    {
        this.book = book;
        this.chapter = chapter;
        this.verse = verse;
        this.endverse = endverse;
    }
    public string GetDisplayText()
    {
        if (verse == endverse)
        {
            return book + " " + chapter + " : " + verse;
        }
        else
        {
            return book + " " + chapter + ":" + verse + "-" + endverse;
        }
    }
}