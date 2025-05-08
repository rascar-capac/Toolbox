using UnityEditor;

namespace Rascar.Toolbox.Editor.EditorWindows
{
    public class CustomEditorWindow<TTab> : EditorWindow where TTab : EditorTab
    {
        private EditorTabSystem<TTab> _tabs;

        // add this in implementations
        // [MenuItem("RascarToolbox/EditorWindows")]
        // public static void ShowWindow()
        // {
        //     GetWindow<CustomEditorWindow<TTab>>();
        // }

        private void SceneView_DuringSceneGui(SceneView sceneView)
        {
            _tabs.DrawSceneGUI(sceneView);
        }

        private void OnEnable()
        {
            _tabs = EditorTabSystem<TTab>.FromReflection();

            SceneView.duringSceneGui += SceneView_DuringSceneGui;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= SceneView_DuringSceneGui;

            _tabs.ClearSelection();
            _tabs = null;
        }

        private void OnGUI()
        {
            _tabs.DrawInspectorGUI();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
