using UnityEngine;
using UnityEngine.Events;

namespace Corelib.SUI
{
    public class SEditorGUILayoutToolbar : SUIElement
    {
        private string[] labels;
        private GUIContent[] contents;
        private int selected;
        private UnityAction<int> onValueChanged;

        public SEditorGUILayoutToolbar(int selected, params string[] labels)
        {
            this.selected = selected;
            this.labels = labels;
        }

        public SEditorGUILayoutToolbar(int selected, params GUIContent[] contents)
        {
            this.selected = selected;
            this.contents = contents;
        }

        public SEditorGUILayoutToolbar OnValueChanged(UnityAction<int> onValueChanged)
        {
            this.onValueChanged = onValueChanged;
            return this;
        }

        public override void Render()
        {
            int newIndex = selected;

            if (contents != null && contents.Length > 0)
                newIndex = GUILayout.Toolbar(selected, contents);
            else if (labels != null && labels.Length > 0)
                newIndex = GUILayout.Toolbar(selected, labels);

            if (newIndex != selected)
            {
                selected = newIndex;
                onValueChanged?.Invoke(newIndex);
            }
        }
    }
}
