using UnityEngine;

public class RangeManager : MonoBehaviour
{
    bool showing = false;


    public void ToggleRanges()
    {
        showing = !showing;


        Turret[] turrets =
            FindObjectsOfType<Turret>();


        foreach (Turret turret in turrets)
        {
            if (showing)
                turret.ShowRange();
            else
                turret.HideRange();
        }
    }
}