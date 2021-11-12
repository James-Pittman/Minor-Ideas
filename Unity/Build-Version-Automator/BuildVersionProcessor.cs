using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

// #######################################################################
// HAVE THIS SCRIPT IN A FOLDER NAMED EDITOR IN YOUR UNITY PROJECT FOLDER.
// #######################################################################

// Simple script to automatically update the version number of a project on each build.
public class BuildVersionProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    // String to hold the initial version of a project.
    private const string initialVersion = "0.0";

    // Makes sure this script runs only when building the project.
    public void OnPreprocessBuild(BuildReport report)
    {
        string currentVersion = FindCurrentVersion();
        UpdateVersion(currentVersion);
    }

    // Returns the current version number.
    private string FindCurrentVersion()
    {
        // Split the Version string in the projects Player settings to find the current version.
        string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');

        // If there is no version set in the Player settings, return initialVersion, otherwise return the current version.
        return currentVersion.Length == 1 ? initialVersion : currentVersion[1];
    }

    // Updates the version number.
    private void UpdateVersion(string version)
    {
        // Parses out the version string retrieved by FindCurrentVersion() into a float
        if (float.TryParse(version, out float versionNumber))
        {
            // Increment the version number and get the date.
            float newVersion = versionNumber + 0.1f;
            string date = DateTime.Now.ToString("d");

            // Set the version in Player settings to the new version and also print version to console.
            PlayerSettings.bundleVersion = string.Format("Version [{0}] - {1}", newVersion, date);
            Debug.Log(PlayerSettings.bundleVersion);
        }
    }
}
