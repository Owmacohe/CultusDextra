using System.Collections.Generic;

public class Stats
{
    public int Day { get; private set; }
    public List<int> Score { get; }
    public List<int> Goal { get; }
    
    List<int> TotalChopped { get; }
    List<int> FaithfulChopped { get; }
    List<int> TraitorsChopped { get; }
    
    List<int> TotalMissed { get; }
    List<int> Early { get; }
    List<int> Escaped { get; }

    public Stats()
    {
        Day = -1;
        
        Score = new List<int>();
        Goal = new List<int>();
        
        TotalChopped = new List<int>();
        FaithfulChopped = new List<int>();
        TraitorsChopped = new List<int>();
        
        TotalMissed = new List<int>();
        Early = new List<int>();
        Escaped = new List<int>();
    }

    public void NewDay()
    {
        Day++;
        
        Score.Add(0);
        Goal.Add(25 + (5 * Day));
        
        TotalChopped.Add(0);
        FaithfulChopped.Add(0);
        TraitorsChopped.Add(0);
        
        TotalMissed.Add(0);
        Early.Add(0);
        Escaped.Add(0);
    }

    public void UpdateAfterChop(bool chopped, bool faithful, bool escaped)
    {
        if (chopped)
        {
            TotalChopped[Day]++;

            if (faithful)
            {
                FaithfulChopped[Day]++;
            }
            else
            {
                TraitorsChopped[Day]++;
            }
        }
        else
        {
            TotalMissed[Day]++;

            if (escaped)
            {
                Escaped[Day]++;
            }
            else
            {
                Early[Day]++;
            }
        }
    }

    string CheckAndList(string description, int amount)
    {
        if (amount > 0)
        {
            return description + amount;
        }

        return "";
    }

    public string ToString()
    {
        string faithfulChopped = (Day > 0) ? "\nFaithful chopped: " + FaithfulChopped[Day] : "";
        string traitorsChopped = CheckAndList("\nTraitors chopped: ", TraitorsChopped[Day]);
        
        string earlyChops = CheckAndList("\nPreemptive chops: ", Early[Day]);
        string traitorsEscaped = CheckAndList("\nTraitors escaped: ", Escaped[Day]);
        
        return
            "Score: " + Score[Day]
            + "\nGoal: " + Goal[Day]
            + "\n\nTotal cultists chopped: " + TotalChopped[Day]
            + faithfulChopped
            + traitorsChopped
            + "\n\nTotal cultists missed: " + TotalMissed[Day]
            + earlyChops
            + traitorsEscaped;
    }
}