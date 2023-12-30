using System;

using BeatSaberMarkupLanguage;

using HMUI;

using IPA.Utilities;

using UnityEngine;
using UnityEngine.EventSystems;

namespace BetterBeatSaber.UI; 

public abstract class FlowController : FlowCoordinator {

    protected static BetterBeatSaber BetterBeatSaber => BetterBeatSaber.Instance;
    
    protected abstract string Title { get; }
    protected virtual bool ShowBackButton { get; } = true;
    
    public FlowController? PreviousFlowController { get; set; }
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) {
        SetTitle(Title);
        showBackButton = ShowBackButton;
        Init(firstActivation, addedToHierarchy);
    }

    protected override void BackButtonWasPressed(ViewController _) {
        Exit();
        if(PreviousFlowController != null)
            PreviousFlowController.DismissFlowCoordinator(this);
        else
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
    }

    protected virtual void Init(bool firstActivation, bool addedToHierarchy) { }
    protected virtual void Exit() { }
    
    #region Set Views

    #region Left

    public T SetLeftView<T>(ViewController.AnimationType animationType = ViewController.AnimationType.In) where T : View<T> {
        var view = View<T>.Instance;
        SetLeftView(view, animationType);
        return view;
    }

    public void SetLeftView<T>(T? view, ViewController.AnimationType? animationType = null) where T : View {
        animationType ??= view != null ? ViewController.AnimationType.In : ViewController.AnimationType.Out;
        SetLeftScreenViewController(view, animationType.Value);
    }

    public void SetLeftView(View? view, ViewController.AnimationType? animationType = null) {
        animationType ??= view != null ? ViewController.AnimationType.In : ViewController.AnimationType.Out;
        SetLeftScreenViewController(view, animationType.Value);
    }
    
    #endregion

    #region Right

    public T SetRightView<T>(ViewController.AnimationType animationType = ViewController.AnimationType.In) where T : View<T> {
        var view = View<T>.Instance;
        SetRightView(view, animationType);
        return view;
    }

    public void SetRightView<T>(T? view, ViewController.AnimationType? animationType = null) where T : View {
        animationType ??= view != null ? ViewController.AnimationType.In : ViewController.AnimationType.Out;
        SetRightScreenViewController(view, animationType.Value);
    }

    public void SetRightView(View? view, ViewController.AnimationType? animationType = null) {
        animationType ??= view != null ? ViewController.AnimationType.In : ViewController.AnimationType.Out;
        SetRightScreenViewController(view, animationType.Value);
    }

    #endregion

    #endregion

    #region Present

    public void PresentFlowController(FlowController flowController) =>
        PresentFlowCoordinator(flowController);

    #endregion

    #region Create

    public static T? CreateFlowController<T>(FlowController? previousFlowController = null) where T : FlowController =>
        (T?) CreateFlowController(typeof(T), previousFlowController);

    public static FlowController? CreateFlowController(Type type, FlowController? previousFlowController = null) {
        var flowController = (FlowController?) new GameObject(type.Name).AddComponent(type);
        DontDestroyOnLoad(flowController);
        // ReSharper disable once Unity.NoNullPropagation
        flowController?.SetField<FlowCoordinator, BaseInputModule>("_baseInputModule", BeatSaberUI.MainFlowCoordinator.GetField<BaseInputModule, FlowCoordinator>("_baseInputModule"));
        // ReSharper disable once Unity.NoNullPropagation
        if(previousFlowController != null)
            flowController?.SetProperty<FlowController, FlowController?>("PreviousFlowController", previousFlowController);
        return flowController;
    }
    
    #endregion
    
}

public abstract class FlowController<T> : FlowController where T : FlowController<T> {

    private static T? _instance;
    public static T Instance {
        get {
            _instance ??= CreateFlowController<T>()!;
            return _instance;
        }
    }

}