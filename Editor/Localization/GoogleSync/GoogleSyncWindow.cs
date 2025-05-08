#if UNITY_LOCALIZATION_INSTALLED
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;
using UnityEngine;

namespace Rascar.Toolbox.Editor.Localization.GoogleSync
{
    public class GoogleSyncWindow : EditorWindow
    {
        private const string GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH = "GoogleSyncWindow/";
        private int _collectionSelectionMask;
        private bool _mustPull = true;
        private bool _mustPush = true;
        private bool _mustRemoveMissingEntries = true;

        private List<StringTableCollection> _allStringTableCollections;
        private List<StringTableCollection> _selectedStringTableCollections = new();
        private string[] _stringTableCollectionNames;
        private SheetsServiceProvider _googleSheetsServiceProvider;

        private void FetchAllTables()
        {
            _allStringTableCollections = LocalizationEditorSettings.GetStringTableCollections().ToList();
            _stringTableCollectionNames = new string[_allStringTableCollections.Count];

            for (int collectionIndex = 0; collectionIndex < _allStringTableCollections.Count; collectionIndex++)
            {
                _stringTableCollectionNames[collectionIndex] = _allStringTableCollections[collectionIndex].TableCollectionName;
            }
        }

        private void FetchGoogleSheetsServiceProvider()
        {
            string[] providerGuids = AssetDatabase.FindAssets("t:SheetsServiceProvider");

            if (providerGuids.Length == 0)
            {
                return;
            }

            _googleSheetsServiceProvider = AssetDatabase.LoadAssetAtPath<SheetsServiceProvider>(AssetDatabase.GUIDToAssetPath(providerGuids[0]));
        }

        private void DrawContents()
        {
            EditorGUILayout.Space(10f);

            _collectionSelectionMask = EditorGUILayout.MaskField("Table Collections", _collectionSelectionMask, _stringTableCollectionNames);

            EditorGUILayout.Space(10f);

            _mustPull = EditorGUILayout.ToggleLeft("Pull - if you modified the google sheets file", _mustPull);
            _mustPush = EditorGUILayout.ToggleLeft("Push - if you added a new entry or if you modified a table in Unity", _mustPush);

            EditorGUILayout.Space(10f);

            _mustRemoveMissingEntries = EditorGUILayout.ToggleLeft("Remove Missing Entries", _mustRemoveMissingEntries);

            EditorGUILayout.Space(10f);

            bool applyButtonIsDisabled = _collectionSelectionMask == 0 || _mustPull == false && _mustPush == false;

            using (new EditorGUI.DisabledGroupScope(applyButtonIsDisabled))
            {
                if (GUILayout.Button("Apply"))
                {
                    UpdateSelectedCollectionList();
                    ApplySynchronization();
                }
            }

            using (new EditorGUI.DisabledGroupScope(_collectionSelectionMask == 0))
            {
                if (GUILayout.Button("Open Google Sheets"))
                {
                    UpdateSelectedCollectionList();

                    if (TryGetGoogleExtension(_selectedStringTableCollections[0], out GoogleSheetsExtension googleExtension))
                    {
                        GoogleSheets.OpenSheetInBrowser(googleExtension.SpreadsheetId);
                    }
                }
            }

            using (new EditorGUI.DisabledGroupScope(_googleSheetsServiceProvider == null))
            {
                if (GUILayout.Button("Open Google Sheets Service Provider"))
                {
                    Selection.activeObject = _googleSheetsServiceProvider;
                }
            }
        }

        private void UpdateSelectedCollectionList()
        {
            _selectedStringTableCollections.Clear();
            IEnumerable<int> selectedCollectionIndices = _collectionSelectionMask.GetBitMaskIndices();

            foreach (int collectionIndex in selectedCollectionIndices)
            {
                _selectedStringTableCollections.Add(_allStringTableCollections[collectionIndex]);
            }
        }

        private void ApplySynchronization()
        {
            if (_mustPull)
            {
                ApplyPullOnSelectedCollections();
            }

            if (_mustPush)
            {
                ApplyPushOnSelectedCollections();
            }
        }

        private void ApplyPushOnSelectedCollections()
        {
            foreach (StringTableCollection collection in _selectedStringTableCollections)
            {
                if (TryGetGoogleExtension(collection, out GoogleSheetsExtension googleExtension))
                {
                    PushCollection(googleExtension);
                }
            }
        }

        private void PushCollection(GoogleSheetsExtension extension)
        {
            GoogleSheets googleSheets = new(extension.SheetsServiceProvider)
            {
                SpreadSheetId = extension.SpreadsheetId
            };
            googleSheets.PushStringTableCollection(extension.SheetId, extension.TargetCollection as StringTableCollection, extension.Columns);
        }

        private void ApplyPullOnSelectedCollections()
        {
            foreach (StringTableCollection collection in _selectedStringTableCollections)
            {
                if (TryGetGoogleExtension(collection, out GoogleSheetsExtension googleExtension))
                {
                    PullCollection(googleExtension);
                }
            }
        }

        private void PullCollection(GoogleSheetsExtension extension)
        {
            GoogleSheets googleSheets = new(extension.SheetsServiceProvider)
            {
                SpreadSheetId = extension.SpreadsheetId
            };
            googleSheets.PullIntoStringTableCollection(extension.SheetId, extension.TargetCollection as StringTableCollection, extension.Columns, _mustRemoveMissingEntries);
        }

        private bool TryGetGoogleExtension(StringTableCollection collection, out GoogleSheetsExtension googleExtension)
        {
            googleExtension = collection.Extensions.FirstOrDefault(extension => extension is GoogleSheetsExtension) as GoogleSheetsExtension;

            if (googleExtension == null)
            {
                Debug.LogError($"String Table Collection {collection.TableCollectionName} does not contain a Google Sheets Extension.");

                return false;
            }

            return true;
        }

        private void OnEnable()
        {
            FetchAllTables();
            FetchGoogleSheetsServiceProvider();

            EditorPrefsUtils.TryGetEditorPrefsInt(GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH + nameof(_collectionSelectionMask), out _collectionSelectionMask);
            EditorPrefsUtils.TryGetEditorPrefsBool(GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH + nameof(_mustPull), out _mustPull);
            EditorPrefsUtils.TryGetEditorPrefsBool(GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH + nameof(_mustPush), out _mustPush);
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt(GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH + nameof(_collectionSelectionMask), _collectionSelectionMask);
            EditorPrefs.SetBool(GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH + nameof(_mustPull), _mustPull);
            EditorPrefs.SetBool(GOOGLE_SYNC_EDITOR_PREFS_ROOT_PATH + nameof(_mustPush), _mustPush);
        }

        private void OnGUI()
        {
            DrawContents();
        }
    }

}
#endif