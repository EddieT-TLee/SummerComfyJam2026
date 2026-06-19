using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueAction))]
public class DialogueActionDrawer : PropertyDrawer
{
    private const float VerticalSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            SerializedProperty actionTypeProperty = property.FindPropertyRelative("actionType");
            SerializedProperty questNameProperty = property.FindPropertyRelative("questName");
            SerializedProperty dialogueProperty = property.FindPropertyRelative("dialogue");

            fieldRect.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;
            EditorGUI.PropertyField(fieldRect, actionTypeProperty);

            fieldRect.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;
            DialogueActionType actionType = (DialogueActionType)actionTypeProperty.intValue;

            switch (actionType)
            {
                case DialogueActionType.StartQuest:
                    EditorGUI.PropertyField(fieldRect, questNameProperty);
                    break;
                case DialogueActionType.ChangeDialogue:
                    EditorGUI.PropertyField(fieldRect, dialogueProperty, new GUIContent("NPC Dialogue"));
                    break;
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lineCount = property.isExpanded ? 3 : 1;
        return lineCount * EditorGUIUtility.singleLineHeight + (lineCount - 1) * VerticalSpacing;
    }
}
