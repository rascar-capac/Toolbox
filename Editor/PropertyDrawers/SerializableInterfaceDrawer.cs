using System;
using System.Collections.Generic;
using System.Text;
using Rascar.Toolbox.Editor.CustomObjectFields;
using Rascar.Toolbox.Extensions;
using Rascar.Toolbox.Serialization;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rascar.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SerializableInterface<>))]
    public class SerializableInterfaceDrawer : PropertyDrawer
    {
        private const string OBJECT_NAME = "_object";
        private const string OBJECT_SELECTOR_UPDATED_COMMAND = "ObjectSelectorUpdated";
        private const string OBJECT_SELECTOR_CLOSED_COMMAND = "ObjectSelectorClosed";

        private readonly Dictionary<string, PropertyCache> _propertyCacheMap = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using EditorGUI.PropertyScope propertyScope = new(position, label, property);

            Type genericType = fieldInfo.FieldType;

            while (genericType.Name != typeof(SerializableInterface<>).Name)
            {
                genericType = genericType.GetGenericArguments()?[0];

                if (genericType == null)
                {
                    EditorGUI.HelpBox(position, "Property type is unknown", MessageType.Error);

                    return;
                }
            }

            genericType = genericType.GetGenericArguments()[0];

            if (genericType.IsInterface)
            {
                PropertyCache propertyCache = GetOrCreatePropertyCache(property);
                propertyCache.InterfaceType = genericType;
                propertyCache.ObjectProperty = property.FindPropertyRelative(OBJECT_NAME);

                propertyCache.ObjectProperty.objectReferenceValue = propertyCache.CustomObjectField.Draw(position, label, propertyCache.ObjectProperty.objectReferenceValue);

                if (EditorGUIUtility.GetObjectPickerControlID() == propertyCache.ObjectPickerControlID)
                {
                    switch (Event.current.commandName)
                    {
                        case OBJECT_SELECTOR_UPDATED_COMMAND:
                        {
                            propertyCache.ObjectProperty.objectReferenceValue = EditorGUIUtility.GetObjectPickerObject();

                            break;
                        }
                        case OBJECT_SELECTOR_CLOSED_COMMAND:
                        {
                            propertyCache.ResetControlID();

                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                EditorGUI.HelpBox(position, "Property is not a interface type", MessageType.Error);
            }
        }

        private static string GetSearchFilter(Type genericType)
        {
            StringBuilder stringBuilder = new();

            foreach (Type type in genericType.GetImplementations())
            {
                stringBuilder.Append(" t:");
                stringBuilder.Append(type.Name);
            }

            return stringBuilder.ToString();
        }

        private PropertyCache GetOrCreatePropertyCache(SerializedProperty property)
        {
            if (!_propertyCacheMap.TryGetValue(property.propertyPath, out PropertyCache propertyCache))
            {
                propertyCache = new PropertyCache();
                _propertyCacheMap.Add(property.propertyPath, propertyCache);
            }

            return propertyCache;
        }

        private sealed class PropertyCache
        {
            public CustomObjectField CustomObjectField { get; }
            public Type InterfaceType { get; set; }
            public SerializedProperty ObjectProperty { get; set; }
            public int ObjectPickerControlID { get; private set; } = -1;

            public PropertyCache()
            {
                CustomObjectField = new(IsObjectValidForField);
                CustomObjectField.OnObjectPickerButtonClicked += CustomObjectField_OnObjectPickerButtonClicked;
            }

            public void ResetControlID()
            {
                ObjectPickerControlID = -1;
            }

            private bool IsObjectValidForField(Object unityObject)
            {
                return InterfaceType.IsAssignableFrom(unityObject.GetType());
            }

            private void CustomObjectField_OnObjectPickerButtonClicked()
            {
                ObjectPickerControlID = GUIUtility.GetControlID(FocusType.Passive);
                EditorGUIUtility.ShowObjectPicker<Object>(ObjectProperty.objectReferenceValue, allowSceneObjects: true, GetSearchFilter(InterfaceType), ObjectPickerControlID);
            }
        }
    }
}
