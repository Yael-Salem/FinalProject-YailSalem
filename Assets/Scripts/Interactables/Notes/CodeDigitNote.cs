using System.Linq;
using UnityEngine;

// Note variant used when the player is piecing together a code using multiple scattered notes
// This note is not added to the player's inventory and tells the UI elements to update in the pause menu with the code to help the player keep track
public class CodeDigitNote : Note
{
   [SerializeField] [Range(0, 3)] private int digitPosition;

   protected override void OnNoteCollected()
   {
      char digitValue = itemData.fullNoteContent.FirstOrDefault(char.IsDigit);

      CodeProgressTrackerUI.RecordDigit(digitPosition, digitValue);
   }
}
