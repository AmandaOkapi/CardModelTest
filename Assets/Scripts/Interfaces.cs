using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public interface IGridObjectAppearance
{
        public void Die();        

}
public interface ITextPrefab{
        public TextMeshProUGUI GetText();
        public void GoalMet();

} 

