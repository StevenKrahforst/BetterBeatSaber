using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;

using BeatSaberMarkupLanguage;

using UnityEngine;

namespace BetterBeatSaber.Manager;

public sealed class ModalManager : Utilities.PersistentSingleton<ModalManager> {

    private static MainMenuViewController? MainMenuViewController => Resources.FindObjectsOfTypeAll<MainMenuViewController>().FirstOrDefault();

    public static bool IsReadyToDisplay => MainMenuViewController != null;
    
    public ConcurrentQueue<Modal> Queue { get; } = new();

    private void Start() =>
        StartCoroutine(DisplayModals());

    protected override void OnDestroy() {
        StopCoroutine(DisplayModals());
        base.OnDestroy();
    }

    private IEnumerator DisplayModals() {
        yield return new WaitUntil(() => IsReadyToDisplay);
        for (;;) {

            yield return new WaitForSeconds(1);
            
            if (Queue.IsEmpty || !Queue.TryDequeue(out var modal) || modal == null || !IsReadyToDisplay)
                continue;

            try {
                var @params = BSMLParser.instance.Parse(modal.BuildContent(), MainMenuViewController!.gameObject);
                @params?.EmitEvent("show");
            } catch (Exception) {
                BetterBeatSaber.Instance.Logger.Warn("Failed to render a Modal");
            }
            
            yield return new WaitForSeconds(3);
            
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void ShowModal(string title, string text) =>
        Queue.Enqueue(new Modal(title, text));
    
    public record Modal(string Title, string Text) {

        public string Title { get; set; } = Title;
        public string Text { get; set; } = Text;

        internal string BuildContent() =>
            $"<modal click-off-closes='true' move-to-center='true' show-event='show'><text text='{Title}' rich-text='true' align='Center' font-size='5' /><text text='{Text}' rich-text='true' /></modal>";

    }

}