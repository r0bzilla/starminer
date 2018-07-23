using UnityEngine;
using System.Collections;

public class SpeedDisplay : MonoBehaviour
{
    public Rigidbody2D ship;

    float speed = 0.0f;

    void Update()
    {
        speed = ship.velocity.magnitude;
    }

    void OnGUI()
    {
        int w = Screen.width;
        int h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.padding.top = h * 2 / 100;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        string text = string.Format("Speed: {0:0.00}", speed);
        GUI.Label(rect, text, style);
    }
}