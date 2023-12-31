﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Styles.Editor
{
    [Serializable]
    public class ImageStyle : BaseStyle<Image>
    {
        [field: SerializeField]
        public Sprite Sprite { get; private set; }
        [field: SerializeField]
        public bool PreserveAspect { get; private set; }
        [field: SerializeField]
        public Image.Type Type { get; private set; }
        [field: SerializeField]
        public Image.FillMethod FillMode { get; private set; }
        [field: SerializeField]
        public bool FillCenter { get; private set; }
        [field: SerializeField]
        public Material Material { get; private set; }
        
        public override void Copy(Image component)
        {
            base.Copy(component);
            Sprite = component.sprite;
            PreserveAspect = component.preserveAspect;
            Type = component.type;
            FillMode = component.fillMethod;
            FillCenter = component.fillCenter;
            Material = component.material;
        }

        public override void Apply(Image component)
        {
            component.sprite = Sprite;
            component.preserveAspect = PreserveAspect;
            component.type = Type;
            component.fillMethod = FillMode;
            component.fillCenter = FillCenter;
            component.material = Material;
        }
    }

    [CreateAssetMenu(fileName = "Image Styles", menuName = "N Ui/Styles/Image", order = 0)]
    public class ImageStyleDatabase : AbstractStyleDatabase<ImageStyle, Image>
    {
        [CustomEditor(typeof(ImageStyleDatabase))]
        public class ImageStyleDatabaseCE : AbstractStyleDatabaseCE<Image>{}

    }
}