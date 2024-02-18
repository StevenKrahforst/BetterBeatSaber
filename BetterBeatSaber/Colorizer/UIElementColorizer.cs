using System.Linq;

using BetterBeatSaber.Extensions;

using HMUI;

using UnityEngine;
using UnityEngine.UI;

namespace BetterBeatSaber.Colorizer; 

public abstract class UIElementColorizer<T> : MonoBehaviour {

    public int amount = -1;
    public float alpha = 1f;
    
    private T[] _images = null!;

    private bool _isImageView;
    
    private void Start() {
        
        _images = GetComponentsInChildren<T>();
        
        if(amount > 0)
            _images = _images.Take(amount).ToArray();
        
        _isImageView = typeof(T) == typeof(ImageView);
        if (!_isImageView)
            return;
        
        foreach (var imageView in _images.Cast<ImageView>())
            imageView.gradient = true;

    }

    private void Update() {

        if (Manager.ColorManager.Instance == null)
            return;
        
        if (_isImageView) {
            foreach (var imageView in _images.Cast<ImageView>()) {
                if (imageView.name == "Icon") {
                    imageView.color0 = Manager.ColorManager.Instance.FirstColor;
                    imageView.color1 = Manager.ColorManager.Instance.SecondColor;
                } else {
                    imageView.color0 = Manager.ColorManager.Instance.FirstColor.WithAlpha(alpha);
                    imageView.color1 = Manager.ColorManager.Instance.SecondColor.WithAlpha(alpha);
                }
            }
        } else {
            foreach (var image in _images.Cast<Image>()) {
                image.color = Manager.ColorManager.Instance.FirstColor;
            }
        }
        
    }

}

public sealed class ImageColorizer : UIElementColorizer<Image>;
public sealed class ImageViewColorizer : UIElementColorizer<ImageView>;