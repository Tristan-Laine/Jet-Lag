using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeDisplay : MonoBehaviour
{
    public TMP_Text modeText;

    void Start()
    {
        Debug.Log("ModeDisplay : " + MainMenu.checkpointsEnabled);

        modeText.text = MainMenu.checkpointsEnabled ? "Avec Checkpoints" : "";
    }
}
