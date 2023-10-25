using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UI.Styles
{
    public abstract class BaseStyle<TComponent> where TComponent : Component
    {
        [field: SerializeField]
        public string Name { get; protected set; }

        public virtual void Copy(TComponent component)
        {
            Name = $"{typeof(TComponent).Name}:{component.name}";
        }
        public abstract void Apply(TComponent component);
    }

    public abstract class BaseStyleDatabase : ScriptableObject
    {
        [field:SerializeField]
        public int Priority { get; private set; }
        public abstract IEnumerable<string> StyleNames { get; }
        public abstract bool CanApply(GameObject target);
        public abstract void Apply(string name, GameObject target);

        public abstract void AddStyle(Component component);
    }

    public abstract class AbstractStyleDatabase<TStyle, TTarget> : BaseStyleDatabase 
        where TStyle : BaseStyle<TTarget>, new()
        where TTarget : Component
    {
        public override IEnumerable<string> StyleNames => Styles.Select(e => e.Name);

        [field:SerializeField]
        public List<TStyle> Styles { get; private set; }

        public override bool CanApply(GameObject target)
        {
            return target.TryGetComponent<TTarget>(out var component);
        }

        public override void Apply(string styleName, GameObject target)
        {
            var style = Styles.FirstOrDefault(e => e.Name == styleName);
            if (style == null)
            {
                Debug.LogError($"Style \"{styleName}\" was not found!");
            }
            if (!target.TryGetComponent<TTarget>(out var targetCmp))
            {
                Debug.LogError($"No component of type {typeof(TTarget)} at {target}", target);
                return;
            }
            Undo.RecordObject(targetCmp, "Revert style");
            style.Apply(targetCmp);
            EditorUtility.SetDirty(targetCmp);
        }

        public override void AddStyle(Component component)
        {
            TStyle style = new TStyle();
            style.Copy((TTarget)component);
            Styles.Add(style);
            Undo.RecordObject(this, "Copying style");
            EditorUtility.SetDirty(this);
        }

        public class AbstractStyleDatabaseCE<TTarget> : UnityEditor.Editor
            where TTarget:Component
        {
            private TTarget _newCmp;

            public override void OnInspectorGUI()
            {
                _newCmp = (TTarget)EditorGUILayout.ObjectField(_newCmp, typeof(TTarget), true);
                base.OnInspectorGUI();
                if (_newCmp)
                {
                    if (GUILayout.Button("Create style from object"))
                    {
                        (target as BaseStyleDatabase).AddStyle(_newCmp);
                    }
                }
            }
        }
    }
}