using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace UI.Styles.Editor
{
    [Serializable]
    public class TextStyle : BaseStyle<TMP_Text>
    {
        [field: SerializeField]
        public TMP_FontAsset Font { get; private set; }
        [field: SerializeField]
        public Material FontMaterial { get; private set; }
        [field: SerializeField]
        public float Size { get; private set; }
        [field: SerializeField]
        public Color Color { get; private set; }

        public override void Copy(TMP_Text component)
        {
            Font = component.font;
            FontMaterial = component.fontSharedMaterial;
            Size = component.fontSize;
            Color = component.color;
        }

        public override void Apply(TMP_Text component)
        {
            component.font = Font;
            component.fontSharedMaterial = FontMaterial;
            component.fontSize = Size;
            component.color = Color;
        }
    }

    [CreateAssetMenu(fileName = "TMP_Text Styles", menuName = "N Ui/Styles/TMP_Text", order = 0)]
    public class TMPTextStyleDatabase : AbstractStyleDatabase<TextStyle, TMP_Text>
    {
        [CustomEditor(typeof(TMPTextStyleDatabase))]
        public class TMPTextStyleDatabaseCE : AbstractStyleDatabaseCE<TMP_Text>{}
    }
}