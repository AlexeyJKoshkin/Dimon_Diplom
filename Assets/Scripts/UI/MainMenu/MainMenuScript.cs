using ShutEye.Extensions;
using ShutEye.UI.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : SEComponentUI
{
    private static MainMenuScript _currentMenu;

    public static void LoadMainMenu()
    {
        if (_currentMenu != null)
        {
            Debug.LogWarning("Current MM != null");
        }
        else
        {
            SceneManager.LoadScene(STRH.DefaultNames.MainMenuScene);
        }
        //TestMainFrame.PrepareGameToLunch("Test Develop");
    }

    public static IEnumerator CloseMainMenu()
    {
        yield return SceneManager.UnloadSceneAsync(STRH.DefaultNames.MainMenuScene);
    }

    public static bool IsActive()
    {
        return !(_currentMenu == null);
    }

    protected override void PrepareUI(Action _onComplete)
    {
        if (_currentMenu != null)
        {
            throw new Exception("Try Instance Main Menu Twice");
        }
        _currentMenu = this;
        base.PrepareUI(_onComplete);
    }

    protected override void OnDestroy()
    {
        _currentMenu = null;
        base.OnDestroy();
    }
}