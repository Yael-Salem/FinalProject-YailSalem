using UnityEngine;

public class LeverProgressNote : Note
{
   protected override void OnNoteCollected()
   {
      if (Lever.LeverPulledCount == 0)
         ObjectiveManager.Instance.TriggerObjective($"override_0");
   }
}
