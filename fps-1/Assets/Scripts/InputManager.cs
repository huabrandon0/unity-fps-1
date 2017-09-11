using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    private static Dictionary<string, KeyCode> keybinds;

    static InputManager()
    {
        InitializeKeybinds();
    }

    private static void InitializeKeybinds()
    {
        keybinds = new Dictionary<string, KeyCode>();
        for (int i = 0; i < defaultKeys.Length; i++)
        {
            keybinds.Add(defaultKeys[i], defaultValues[i]);
        }
    }

    private static string[] defaultKeys = new string[]
    {
        "Attack1"
    };

    private static KeyCode[] defaultValues = new KeyCode[]
    {
        KeyCode.Mouse0
    };

    public static bool GetKeyDown(string key)
    {
        return Input.GetKeyDown(keybinds[key]);
    }
}
