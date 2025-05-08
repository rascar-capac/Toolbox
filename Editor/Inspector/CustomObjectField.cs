using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rascar.Toolbox.Editor.CustomObjectFields
{
    /// <summary>
    /// Used to draw customized object field with more functionality than <see cref="EditorGUILayout.ObjectField"/>.
    /// </summary>
    public class CustomObjectField
    {
        private readonly RectOffset _pickerRectOffset = new(-1, 0, -1, -1);
        private readonly RectOffset _iconRectOffset = new(-2, 0, -2, -2);
        private readonly Predicate<Object> _validForFieldPredicate;
        private readonly Func<Object, GUIContent> _objectLabelGetter;

        public Rect FieldRect { get; private set; }

        public event Action OnObjectPickerButtonClicked;

        public CustomObjectField(Predicate<Object> isObjectValidForField, Func<Object, GUIContent> objectLabelGetter = null)
        {
            _validForFieldPredicate = isObjectValidForField;
            _objectLabelGetter = objectLabelGetter ?? DefaultObjectLabelGetter;
        }

        public Object Draw(Rect position, GUIContent label, Object activeObject)
        {
            Rect objectRect;

            using (new EditorGUI.IndentLevelScope(0))
            {
                objectRect = EditorGUI.PrefixLabel(position, label);
            }

            Rect pickerRect = objectRect;
            pickerRect.xMin = position.xMax - 20f;
            pickerRect = _pickerRectOffset.Add(pickerRect);

            if (Event.current.type == EventType.Repaint)
            {
                FieldRect = objectRect;
            }

            if (GUI.enabled &&
                (HandleObjectPickerEvent(pickerRect)
                || HandleObjectPingEvent(objectRect, activeObject)
                || HandleDragEvents(objectRect, GetDraggedObjectIfValid(), ref activeObject)))
            {
                Event.current.Use();
            }

            GUIContent activeObjectLabel = _objectLabelGetter(activeObject);

            if (activeObjectLabel.image != null)
            {
                DrawObjectField(objectRect, GUIContent.none);

                Rect iconRect = objectRect;
                iconRect.width = iconRect.height;
                iconRect = _iconRectOffset.Add(iconRect);

                Rect labelRect = objectRect;
                labelRect.xMin = iconRect.xMax + 1f;

                Texture icon = activeObjectLabel.image;
                activeObjectLabel.image = null;

                using (new EditorGUI.IndentLevelScope(0))
                {
                    EditorGUI.LabelField(labelRect, activeObjectLabel, Styles.OBJECT_LABEL);
                }

                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }
            else
            {
                DrawObjectField(objectRect, activeObjectLabel);
            }

            GUI.Button(pickerRect, GUIContent.none, Styles.PICKER);

            return activeObject;
        }

        private void DrawObjectField(Rect objectRect, GUIContent label)
        {
            bool highlight = objectRect.Contains(Event.current.mousePosition) && HasValidDraggedObject();
            GUI.Toggle(objectRect, highlight, label, Styles.OBJECT_FIELD);
        }

        private Object GetPingableObject(Object activeObject)
        {
            if (activeObject is Component component)
            {
                return component.gameObject;
            }
            else
            {
                return activeObject;
            }
        }

        private Object GetDraggedObjectIfValid()
        {
            Object[] draggedObjects = DragAndDrop.objectReferences;

            if (draggedObjects.Length != 1)
            {
                return null;
            }

            Object draggedObject = draggedObjects[0];

            return _validForFieldPredicate.Invoke(draggedObject) ? draggedObject : null;
        }

        private bool HasValidDraggedObject()
        {
            return GetDraggedObjectIfValid() != null;
        }

        private bool HandleObjectPickerEvent(Rect buttonRect)
        {
            Event currentEvent = Event.current;

            if (currentEvent.type != EventType.MouseDown)
            {
                return false;
            }

            bool isMouseOverSelectButton = buttonRect.Contains(currentEvent.mousePosition);

            if (isMouseOverSelectButton)
            {
                OnObjectPickerButtonClicked?.Invoke();

                return true;
            }

            return false;
        }

        private bool HandleObjectPingEvent(Rect buttonRect, Object activeObject)
        {
            Event currentEvent = Event.current;

            if (currentEvent.type != EventType.MouseDown || activeObject == null)
            {
                return false;
            }

            bool isMouseOverSelectButton = buttonRect.Contains(currentEvent.mousePosition);

            if (isMouseOverSelectButton)
            {
                EditorGUIUtility.PingObject(GetPingableObject(activeObject));

                return true;
            }

            return false;
        }

        private static bool HandleDragEvents(Rect objectRect, bool isValidObjectBeingDragged, ref Object activeObject)
        {
            bool isMouseOverObjectField = objectRect.Contains(Event.current.mousePosition);

            if (!isMouseOverObjectField)
            {
                return false;
            }

            EventType eventType = Event.current.type;

            switch (eventType)
            {
                case EventType.DragUpdated:
                {
                    DragAndDrop.visualMode = isValidObjectBeingDragged ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;

                    return true;
                }
                case EventType.DragPerform:
                {
                    if (isValidObjectBeingDragged)
                    {
                        DragAndDrop.AcceptDrag();
                        activeObject = DragAndDrop.objectReferences[0];
                    }

                    return true;
                }
                case EventType.DragExited:
                {
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }

        public static GUIContent DefaultObjectLabelGetter(Object unityObject)
        {
            return unityObject ? new GUIContent(unityObject.ToString(), AssetPreview.GetMiniThumbnail(unityObject)) : new GUIContent("None");
        }

        private static class Styles
        {
            public static readonly GUIStyle OBJECT_FIELD = EditorStyles.objectField;
            public static readonly GUIStyle PICKER = new("ObjectFieldButton");
            public static readonly GUIStyle OBJECT_LABEL = CreateObjectLabelStyle();

            private static GUIStyle CreateObjectLabelStyle()
            {
                GUIStyle labelStyle = new(EditorStyles.objectField);
                labelStyle.normal.background = Texture2D.blackTexture;

                return labelStyle;
            }
        }
    }
}
