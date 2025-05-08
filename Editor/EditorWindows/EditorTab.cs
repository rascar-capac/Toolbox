using UnityEditor;

namespace Rascar.Toolbox.Editor.EditorWindows
{
    public abstract class EditorTab
    {
        public abstract string Name { get; }

        public virtual void DrawInspectorGUI() { }

        public virtual void DrawSceneGUI(SceneView sceneView) { }

        public virtual void OnSelected() { }

        public virtual void OnDeselected() { }
    }
}