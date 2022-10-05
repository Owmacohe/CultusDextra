public class Stats
{
    /*
    TMP_Text totalCultistsChopped, faithfulChopped, traitorsChopped;
    TMP_Text cultistsMissed, traitorsEscaped;
    TMP_Text anticipationBuildup;
    */

    public int TotalChopped { get; private set; }
    public int FaithfulChopped { get; private set; }
    public int TraitorsChopped { get; private set; }
    public int TotalMissed { get; private set; }
    public int TotalEscaped { get; private set; }
    public float AnticipationBuildup { get; private set; }

    public void UpdateAfterChop(bool chopped, bool faithful, bool escaped)
    {
        if (chopped)
        {
            TotalChopped++;

            if (faithful)
            {
                FaithfulChopped++;
            }
            else
            {
                TraitorsChopped++;
            }
        }
        else
        {
            TotalMissed++;

            if (escaped)
            {
                TotalEscaped++;
            }
        }
    }
}