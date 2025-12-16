using UnityEngine;
using TMPro;

public class UI_counter : MonoBehaviour
{
    public TMP_Text alienCounterText;

    void Update()
    {
        if (AlienManager.instance == null) return;

        int alienCount = AlienManager.instance.aliens.Count;
        alienCounterText.text = alienCount.ToString();
    }
}
