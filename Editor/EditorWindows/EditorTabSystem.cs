using System;
using System.Collections.Generic;
using Rascar.Toolbox.Utilities;
using UnityEditor;
using UnityEngine;

namespace Rascar.Toolbox.Editor.EditorWindows
{
    public class EditorTabSystem<TTab> where TTab : EditorTab
    {
        private readonly List<TTab> _tabs = new();

        private int _currentTabIndex = -1;
        private bool _hasScrollView;
        private Vector2 _scrollPosition;

        public bool HasCurrentTab => _currentTabIndex != -1;
        public TTab CurrentTab => _tabs[_currentTabIndex];

        public EditorTabSystem(bool hasScrollView, IEnumerable<TTab> tabs = null)
        {
            _hasScrollView = hasScrollView;

            foreach (TTab tab in tabs)
            {
                _tabs.Add(tab);
            }
        }

        public static EditorTabSystem<TTab> FromReflection(bool hasScrollView = true, object[] parameters = null, Func<Type, bool> implementationPredicate = null)
        {
            return new EditorTabSystem<TTab>(hasScrollView, ReflectionUtils.CreateInstances<TTab>(parameters, implementationPredicate));
        }

        public void ClearSelection()
        {
            if (HasCurrentTab)
            {
                CurrentTab.OnDeselected();
                _currentTabIndex = -1;
            }
        }

        public void AddTab(TTab tab)
        {
            _tabs.Add(tab);
        }

        public void SelectTabByIndex(int index)
        {
            if (index < 0 || index >= _tabs.Count)
            {
                return;
            }

            if (_currentTabIndex != index)
            {
                if (HasCurrentTab)
                {
                    CurrentTab.OnDeselected();
                }

                _currentTabIndex = index;
                CurrentTab.OnSelected();
            }
        }

        public void SelectTabByName(string tabName)
        {
            int tabIndex = GetTabIndex(tabName);

            if (tabIndex != -1)
            {
                SelectTabByIndex(tabIndex);
            }
        }

        public int GetTabIndex(string tabName)
        {
            return _tabs.FindIndex(tab => tab.Name.Equals(tabName, StringComparison.OrdinalIgnoreCase));
        }

        public bool TryGetTab<TTabChild>(out TTabChild resultTab) where TTabChild : TTab
        {
            resultTab = default;
            TTab tab = _tabs.Find(tab => tab is TTabChild);

            if (tab == null)
            {
                return false;
            }

            resultTab = tab as TTabChild;

            return true;
        }

        public void DrawInspectorGUI()
        {
            DrawTabSelection();

            if (_hasScrollView)
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                CurrentTab.DrawInspectorGUI();
                EditorGUILayout.EndScrollView();
            }
            else
            {
                CurrentTab.DrawInspectorGUI();
            }
        }

        public void DrawSceneGUI(SceneView sceneView)
        {
            if (HasCurrentTab)
            {
                CurrentTab.DrawSceneGUI(sceneView);
            }
        }

        private void DrawTabSelection()
        {
            string[] tabNames = new string[_tabs.Count];

            for (int tabIndex = 0; tabIndex < _tabs.Count; tabIndex++)
            {
                tabNames[tabIndex] = _tabs[tabIndex].Name;
            }

            int newIndex = Mathf.Clamp(EditorGUILayout.Popup("Tab", _currentTabIndex, tabNames), 0, _tabs.Count - 1);
            SelectTabByIndex(newIndex);
        }
    }
}
