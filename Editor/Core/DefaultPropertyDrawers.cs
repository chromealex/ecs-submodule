using System.Collections;
using System.Collections.Generic;
using ME.ECSEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEditor;

namespace ME.ECSEditor {

    [UnityEditor.CustomPropertyDrawer(typeof(Quaternion))]
    public class RotationPropertyDrawer : UnityEditor.PropertyDrawer {

        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label) {

            var rot = property;
            var euler = rot.quaternionValue.eulerAngles;
            euler = UnityEditor.EditorGUI.Vector3Field(position, "Rotation", euler);
            rot.quaternionValue = Quaternion.Euler(euler);

        }

    }

    [CustomPropertyDrawer(typeof(ME.ECS.IsBitmask))]
    public class EnumMaskPropertyDrawer : PropertyDrawer {

        bool foldoutOpen = false;

        object theEnum;
        System.Array enumValues;
        System.Type enumUnderlyingType;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (foldoutOpen)
                return EditorGUIUtility.singleLineHeight * (System.Enum.GetValues(fieldInfo.FieldType).Length + 2);
            else
                return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            theEnum = fieldInfo.GetValue(property.serializedObject.targetObject);
            enumValues = System.Enum.GetValues(theEnum.GetType());
            enumUnderlyingType = System.Enum.GetUnderlyingType(theEnum.GetType());

            //We need to convert the enum to its underlying type, if we don't it will be boxed
            //into an object later and then we would need to unbox it like (UnderlyingType)(EnumType)theEnum.
            //If we do this here we can just do (UnderlyingType)theEnum later (plus we can visualize the value of theEnum in VS when debugging)
            theEnum = System.Convert.ChangeType(theEnum, enumUnderlyingType);

            EditorGUI.BeginProperty(position, label, property);

            foldoutOpen = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), foldoutOpen, label);

            if (foldoutOpen) {
                //Draw the All button
                if (GUI.Button(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 1, 30, 15), "All")) {
                    theEnum = DoNotOperator(System.Convert.ChangeType(0, enumUnderlyingType), enumUnderlyingType);
                }

                //Draw the None button
                if (GUI.Button(new Rect(position.x + 32, position.y + EditorGUIUtility.singleLineHeight * 1, 40, 15), "None")) {
                    theEnum = System.Convert.ChangeType(0, enumUnderlyingType);
                }

                //Draw the list
                for (int i = 0; i < System.Enum.GetNames(fieldInfo.FieldType).Length; i++) {
                    if (EditorGUI.Toggle(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (2 + i), position.width, EditorGUIUtility.singleLineHeight),
                                         System.Enum.GetNames(fieldInfo.FieldType)[i], IsSet(i))) {
                        ToggleIndex(i, true);
                    } else {
                        ToggleIndex(i, false);
                    }
                }
            }

            fieldInfo.SetValue(property.serializedObject.targetObject, theEnum);
            property.serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Get the value of an enum element at the specified index (i.e. at the index of the name of the element in the names array)
        /// </summary>
        object GetEnumValue(int _index) {
            return System.Convert.ChangeType(enumValues.GetValue(_index), enumUnderlyingType);
        }

        /// <summary>
        /// Sets or unsets a bit in theEnum based on the index of the enum element (i.e. the index of the element in the names array)
        /// </summary>
        /// <param name="_set">If true the flag will be set, if false the flag will be unset.</param>
        void ToggleIndex(int _index, bool _set) {
            if (_set) {
                if (IsNoneElement(_index)) {
                    theEnum = System.Convert.ChangeType(0, enumUnderlyingType);
                }

                //enum = enum | val
                theEnum = DoOrOperator(theEnum, GetEnumValue(_index), enumUnderlyingType);
            } else {
                if (IsNoneElement(_index) || IsAllElement(_index)) {
                    return;
                }

                object val = GetEnumValue(_index);
                object notVal = DoNotOperator(val, enumUnderlyingType);

                //enum = enum & ~val
                theEnum = DoAndOperator(theEnum, notVal, enumUnderlyingType);
            }

        }

        /// <summary>
        /// Checks if a bit flag is set at the provided index of the enum element (i.e. the index of the element in the names array)
        /// </summary>
        bool IsSet(int _index) {
            object val = DoAndOperator(theEnum, GetEnumValue(_index), enumUnderlyingType);

            //We handle All and None elements differently, since they're "special"
            if (IsAllElement(_index)) {
                //If all other bits visible to the user (elements) are set, the "All" element checkbox has to be checked
                //We don't do a simple AND operation because there might be missing bits.
                //e.g. An enum with 6 elements including the "All" element. If we set all bits visible except the "All" bit,
                //two bits might be unset. Since we want the "All" element checkbox to be checked when all other elements are set
                //we have to make sure those two extra bits are also set.
                bool allSet = true;
                for (int i = 0; i < System.Enum.GetNames(fieldInfo.FieldType).Length; i++) {
                    if (i != _index && !IsNoneElement(i) && !IsSet(i)) {
                        allSet = false;
                        break;
                    }
                }

                //Make sure all bits are set if all "visible bits" are set
                if (allSet) {
                    theEnum = DoNotOperator(System.Convert.ChangeType(0, enumUnderlyingType), enumUnderlyingType);
                }

                return allSet;
            } else if (IsNoneElement(_index)) {
                //Just check the "None" element checkbox our enum's value is 0
                return System.Convert.ChangeType(theEnum, enumUnderlyingType).Equals(System.Convert.ChangeType(0, enumUnderlyingType));
            }

            return !val.Equals(System.Convert.ChangeType(0, enumUnderlyingType));
        }

        /// <summary>
        /// Call the bitwise OR operator (|) on _lhs and _rhs given their types.
        /// Will basically return _lhs | _rhs
        /// </summary>
        /// <param name="_lhs">Left-hand side of the operation.</param>
        /// <param name="_rhs">Right-hand side of the operation.</param>
        /// <param name="_type">Type of the objects.</param>
        /// <returns>Result of the operation</returns>
        static object DoOrOperator(object _lhs, object _rhs, System.Type _type) {
            if (_type == typeof(int)) {
                return ((int)_lhs) | ((int)_rhs);
            } else if (_type == typeof(uint)) {
                return ((uint)_lhs) | ((uint)_rhs);
            } else if (_type == typeof(short)) {
                //ushort and short don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((short)((short)_lhs | (short)_rhs));
            } else if (_type == typeof(ushort)) {
                //ushort and short don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((ushort)((ushort)_lhs | (ushort)_rhs));
            } else if (_type == typeof(long)) {
                return ((long)_lhs) | ((long)_rhs);
            } else if (_type == typeof(ulong)) {
                return ((ulong)_lhs) | ((ulong)_rhs);
            } else if (_type == typeof(byte)) {
                //byte and sbyte don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((byte)((byte)_lhs | (byte)_rhs));
            } else if (_type == typeof(sbyte)) {
                //byte and sbyte don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((sbyte)((sbyte)_lhs | (sbyte)_rhs));
            } else {
                throw new System.ArgumentException("Type " + _type.FullName + " not supported.");
            }
        }

        /// <summary>
        /// Call the bitwise AND operator (&) on _lhs and _rhs given their types.
        /// Will basically return _lhs & _rhs
        /// </summary>
        /// <param name="_lhs">Left-hand side of the operation.</param>
        /// <param name="_rhs">Right-hand side of the operation.</param>
        /// <param name="_type">Type of the objects.</param>
        /// <returns>Result of the operation</returns>
        static object DoAndOperator(object _lhs, object _rhs, System.Type _type) {
            if (_type == typeof(int)) {
                return ((int)_lhs) & ((int)_rhs);
            } else if (_type == typeof(uint)) {
                return ((uint)_lhs) & ((uint)_rhs);
            } else if (_type == typeof(short)) {
                //ushort and short don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((short)((short)_lhs & (short)_rhs));
            } else if (_type == typeof(ushort)) {
                //ushort and short don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((ushort)((ushort)_lhs & (ushort)_rhs));
            } else if (_type == typeof(long)) {
                return ((long)_lhs) & ((long)_rhs);
            } else if (_type == typeof(ulong)) {
                return ((ulong)_lhs) & ((ulong)_rhs);
            } else if (_type == typeof(byte)) {
                return unchecked((byte)((byte)_lhs & (byte)_rhs));
            } else if (_type == typeof(sbyte)) {
                //byte and sbyte don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((sbyte)((sbyte)_lhs & (sbyte)_rhs));
            } else {
                throw new System.ArgumentException("Type " + _type.FullName + " not supported.");
            }
        }

        /// <summary>
        /// Call the bitwise NOT operator (~) on _lhs given its type.
        /// Will basically return ~_lhs
        /// </summary>
        /// <param name="_lhs">Left-hand side of the operation.</param>
        /// <param name="_type">Type of the object.</param>
        /// <returns>Result of the operation</returns>
        static object DoNotOperator(object _lhs, System.Type _type) {
            if (_type == typeof(int)) {
                return ~(int)_lhs;
            } else if (_type == typeof(uint)) {
                return ~(uint)_lhs;
            } else if (_type == typeof(short)) {
                //ushort and short don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((short)~(short)_lhs);
            } else if (_type == typeof(ushort)) {

                //ushort and short don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((ushort)~(ushort)_lhs);
            } else if (_type == typeof(long)) {
                return ~(long)_lhs;
            } else if (_type == typeof(ulong)) {
                return ~(ulong)_lhs;
            } else if (_type == typeof(byte)) {
                //byte and sbyte don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return (byte)~(byte)_lhs;
            } else if (_type == typeof(sbyte)) {
                //byte and sbyte don't have bitwise operators, it is automatically converted to an int, so we convert it back
                return unchecked((sbyte)~(sbyte)_lhs);
            } else {
                throw new System.ArgumentException("Type " + _type.FullName + " not supported.");
            }
        }

        /// <summary>
        /// Check if the element of specified index is a "None" element (all bits unset, value = 0).
        /// </summary>
        /// <param name="_index">Index of the element.</param>
        /// <returns>If the element has all bits unset or not.</returns>
        bool IsNoneElement(int _index) {
            return GetEnumValue(_index).Equals(System.Convert.ChangeType(0, enumUnderlyingType));
        }

        /// <summary>
        /// Check if the element of specified index is an "All" element (all bits set, value = ~0).
        /// </summary>
        /// <param name="_index">Index of the element.</param>
        /// <returns>If the element has all bits set or not.</returns>
        bool IsAllElement(int _index) {
            object elemVal = GetEnumValue(_index);
            return elemVal.Equals(DoNotOperator(System.Convert.ChangeType(0, enumUnderlyingType), enumUnderlyingType));
        }

    }

}