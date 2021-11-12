using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.UI;

public class BuildVersionManager : EditorWindow , IPreprocessBuildWithReport
{
    // Increment related attributes
    int incrementIndex = 2;
    float incrementVal = 0.1f;
    string[] incrementOptions = new string[] { "0.01", "0.05", "0.1", "0.5" };
    bool autoIncrement = true;
    bool customIncrement = false;

    // Build version related attributes
    bool releaseVersion = false;
    bool initialVersion = false;
    bool customBuild = true;
    string manualVersion;

    // Flag for including date
    bool dateToggle = false;

    // Building flag
    bool isBuilding = false;

    [MenuItem("Window/BuildVersionManager")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BuildVersionManager));
    }


    private void OnGUI()
    {
        // Display build details at top.
        EditorGUILayout.Space();
        GUILayout.Label("Build Details:", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Company Name: ", PlayerSettings.companyName);
        EditorGUILayout.LabelField("Product Name: ", PlayerSettings.productName);
        EditorGUILayout.LabelField("Version: ", PlayerSettings.bundleVersion);
        EditorGUI.indentLevel--;

        // Player settings button to manually change details.
        if (GUILayout.Button("Open Player Settings"))
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
        }

        // If a release build, set the release version manually.
        releaseVersion = EditorGUILayout.Toggle("Set Release Version", releaseVersion);
        if (releaseVersion)
        {
            EditorGUILayout.HelpBox("If you are changing the version number, override the current one below.", MessageType.Warning);
            manualVersion = EditorGUILayout.TextField("Override Version Number: ", manualVersion);
            if (GUILayout.Button("Set"))
            {
                PlayerSettings.bundleVersion = manualVersion;
            }

        }

        // If the intial build, set the release version to "0.0"
        initialVersion = EditorGUILayout.Toggle("Set Initial Version", initialVersion);
        if (initialVersion)
        {
            EditorGUILayout.HelpBox("If you are setting this build as initial, the version number will be reset to 0.0", MessageType.Warning);
            if (GUILayout.Button("Set"))
            {
                PlayerSettings.bundleVersion = "0.0";
                initialVersion = false;
            }

        }

        EditorGUILayout.Space();
        GUILayout.Label("Version Manager Settings:", EditorStyles.boldLabel);

        // Auto-Increment Settings.
        autoIncrement = EditorGUILayout.Toggle("Enable Auto-Increment", autoIncrement);
        if (autoIncrement)
        {
            incrementIndex = EditorGUILayout.Popup("Increment Size", incrementIndex, incrementOptions);
            setIncrement();

            customIncrement = EditorGUILayout.Toggle("Custom Increment Value", customIncrement);
            if (customIncrement)
            {
                incrementVal = EditorGUILayout.DelayedFloatField("Enter Float Value", incrementVal);
            }
        }

        // Toggle for whether to include date or not.
        dateToggle = EditorGUILayout.Toggle("Include Date in Version", dateToggle);

        // Current build preview
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Build Preview:", UpdateVersion(FindCurrentVersion()));
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("New Version will be Automatically Formatted", MessageType.Info);
        EditorGUILayout.Space();
    }

    void setIncrement()
    {
        switch(incrementIndex)
        {
            case 0:
                incrementVal = 0.01f;
                break;
            case 1:
                incrementVal = 0.05f;
                break;
            case 2:
                incrementVal = 0.1f;
                break;
            case 3:
                incrementVal = 0.5f;
                break;
            default:
                Debug.LogError("Unrecognized Increment Index");
                break;
        }
    }

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        string currentVersion = FindCurrentVersion();
        isBuilding = true;
        UpdateVersion(currentVersion);
    }

    // Returns the current version number.
    private string FindCurrentVersion()
    {
        if (customBuild)
        {
            return PlayerSettings.bundleVersion;
        }
        // Split the Version string in the projects Player settings to find the current version.
        string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');

        // If there is no version set in the Player settings, return initialVersion, otherwise return the current version.
        return currentVersion.Length == 1 ? "0.0" : currentVersion[1];

    }

    // Updates the version number.
    private string UpdateVersion(string version)
    {
        string final = null;
        // Parses out the version string retrieved by FindCurrentVersion() into a float
        if (float.TryParse(version, out float versionNumber))
        {
            float newVersion = versionNumber;
            // Increment the version number and get the date.
            if (!releaseVersion)
            {
                newVersion = versionNumber + incrementVal;
            }
            string date = DateTime.Now.ToString("d");

            // Set the version in Player settings to the new version and also print version to console.
            if (dateToggle)
                final = string.Format("Version [{0}] - {1}", newVersion, date);
            else
                final = string.Format("Version [{0}]", newVersion);
        }
        if (!isBuilding)
            return final;
        else
            return PlayerSettings.bundleVersion = final;
    }
}
