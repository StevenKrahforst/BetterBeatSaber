using System;
using System.Collections;
using System.Linq;
using System.Reflection;

using BetterBeatSaber.Extensions;

using IPA.Loader;

using TMPro;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Bindings;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class MenuButtonColorizer : IInitializable, ITickable {

    private Type? _menuButtonsViewControllerType;
    private TextMeshProUGUI? _text;
    
    public void Initialize() {
        _menuButtonsViewControllerType = PluginManager.GetPluginFromId("BeatSaberMarkupLanguage").Assembly.GetType("BeatSaberMarkupLanguage.MenuButtons.MenuButtonsViewController");
        if(_menuButtonsViewControllerType != null)
            SharedCoroutineStarter.instance.StartCoroutine(FindText());
    }
    
    public void Tick() {
        if(_text != null)
            _text.ApplyGradient(Manager.ColorManager.Instance.FirstColor, Manager.ColorManager.Instance.ThirdColor);
    }
    
    private IEnumerator FindText() {

        object? menuButtonsViewController = null;
        yield return new WaitUntil(() => {
            menuButtonsViewController = Resources.FindObjectsOfTypeAll(_menuButtonsViewControllerType).FirstOrDefault();
            return menuButtonsViewController != null;
        });

        GameObject? rootObject = null;
        yield return new WaitUntil(() => {
            rootObject = (GameObject?) _menuButtonsViewControllerType?.GetField("rootObject", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(menuButtonsViewController);
            return rootObject != null;
        });

        _text = rootObject?.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(text => text.text == "Better Beat Saber");
        
        if(_text != null)
            // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
            _text.fontStyle |= FontStyles.Bold;
        
    }

}