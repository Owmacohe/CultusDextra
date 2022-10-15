using System.Collections.Generic;

public class Stats
{
    public int Day { get; set; }
    public List<int> Score { get; set; }
    public List<int> Goal { get; set; }
    
    public List<int> TotalChopped { get; private set; }
    public List<int> FaithfulChopped { get; private set; }
    public List<int> TraitorsChopped { get; private set; }
    
    public List<int> TotalMissed { get; private set; }
    public List<int> TotalEscaped { get; private set; }

    public Stats()
    {
        Day = -1;
        
        Score = new List<int>();
        Goal = new List<int>();
        
        TotalChopped = new List<int>();
        FaithfulChopped = new List<int>();
        TraitorsChopped = new List<int>();
        
        TotalMissed = new List<int>();
        TotalEscaped = new List<int>();
    }

    public void NewDay()
    {
        Day++;
        
        Score.Add(0);
        Goal.Add(0);
        
        TotalChopped.Add(0);
        FaithfulChopped.Add(0);
        TraitorsChopped.Add(0);
        
        TotalMissed.Add(0);
        TotalEscaped.Add(0);
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
                TotalEscaped[Day]++;
            }
        }
    }

    public string ToString()
    {
        return
            "Score: " + Score[Day]
            + "\nGoal: " + Goal[Day]
            + "\n\nTotal cultists chopped: " + TotalChopped[Day]
            + "\nFaithful chopped: " + FaithfulChopped[Day]
            + "\nTraitors chopped: " + TraitorsChopped[Day]
            + "\n\nCultists missed: " + TotalMissed[Day]
            + "\nTraitors escaped: " + TotalEscaped[Day];
    }
}