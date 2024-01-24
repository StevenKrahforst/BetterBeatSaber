using System;
using System.Collections;

using BeatSaberMarkupLanguage.Components;

namespace BetterBeatSaber.UI; 

public abstract class AsyncListView<T, TC> : ListView<T, TC> where TC : ListCell<T> {

    public bool IsLoading { get; private set; }
    
    public event Action? OnLoadingStarted;
    public event Action? OnLoadingFinished;

    protected AsyncListView(CustomListTableData table, bool reload = true) : base(table, reload) { }
    
    public override void LoadItems() {
        IsLoading = true;
        OnLoadingStarted?.Invoke();
        Utilities.SharedCoroutineStarter.Instance.StartCoroutine(LoadWithEvents());
    }

    private IEnumerator LoadWithEvents() {
        yield return Load();
        IsLoading = false;
        OnLoadingFinished?.Invoke();
        Reload();
    }
    
    protected abstract IEnumerator Load();

}