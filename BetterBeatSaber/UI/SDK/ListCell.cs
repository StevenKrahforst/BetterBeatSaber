using HMUI;

namespace BetterBeatSaber.UI; 

public abstract class ListCell<T> : TableCell {

    public abstract void Populate(T data);

}