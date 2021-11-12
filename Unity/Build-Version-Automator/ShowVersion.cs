using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Simple script to retrieve the version number and display it on the UI.
//
// Attach this script to a text element in Unity.
public class ShowVersion : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out Text output))
            output.text = Application.version;
    }
}
