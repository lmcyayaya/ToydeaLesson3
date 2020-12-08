using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {

	public override void OnGUI(Rect position,SerializedProperty property,GUIContent label){
		EditorGUI.PrefixLabel(position,label);
		Rect newposition = position;
		newposition.y += 18f;
		SerializedProperty data = property.FindPropertyRelative("rows");
        if (data.arraySize != 13)
            data.arraySize = 13;
		//data.rows[0][]
		for(int j=0;j<13;j++){
			SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
			newposition.height = 18f;
			if(row.arraySize != 13)
				row.arraySize = 13;
			newposition.width = position.width/13;
			for(int i=0;i<13;i++){
				EditorGUI.PropertyField(newposition,row.GetArrayElementAtIndex(i),GUIContent.none);
				newposition.x += newposition.width;
			}

			newposition.x = position.x;
			newposition.y += 18f;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property,GUIContent label){
		return 18f * 16;
	}
}
