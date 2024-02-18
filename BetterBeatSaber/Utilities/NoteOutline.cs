using System.Linq;

using UnityEngine;

namespace BetterBeatSaber.Utilities;

public sealed class NoteOutline : Outline {

    protected override void UpdateRenderers() =>
        Renderers = gameObject.GetComponentsInChildren<MeshRenderer>().Where(r => r.gameObject.name == "NoteCube");

}