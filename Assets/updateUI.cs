using UnityEngine;
using System;
using TMPro;

public class updateUI : MonoBehaviour {
    public Transform player;
    public TMP_Text distanceText;


    void Update() {
        distanceText.text = player.position.x.ToString("0");
    }
}
