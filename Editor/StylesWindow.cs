using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UI.Styles.Editor
{
    public class StylesWindow : EditorWindow
    {
        [MenuItem("Tools/N UI Styles")]
        public static void CreateStylesWindow()
        {
            var wnd = CreateInstance<StylesWindow>();
            wnd.Show();
        }

        private void OnEnable()
        {
            EditorApplication.update += Repaint;
            EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Repaint;
            EditorApplication.hierarchyWindowChanged -= OnHierarchyWindowChanged;
        }

        private void OnHierarchyWindowChanged()
        {
            Repaint();
        }

        private ProjectStyles _projectStyles;
        private Vector2 _scroll;

        private void OnGUI()
        {
            _projectStyles = (ProjectStyles)EditorGUILayout.ObjectField("Styles", _projectStyles, typeof(ProjectStyles), false);
            if (!_projectStyles)
            {
                return;
            }

            GUILayout.Space(50);

            if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
            {
                EditorGUILayout.HelpBox("Select any objects!", MessageType.Info);
                return;
            }

            EditorGUILayout.BeginVertical("Box");
            _scroll = EditorGUILayout.BeginScrollView(_scroll, false, false);
            var filterStyles = _projectStyles
                .Styles
                .Where(e => 
                    Selection.gameObjects.All(go => e.CanApply(go))
                )
                .OrderBy(e=>e.Priority).ToArray();

            if (filterStyles.Any())
            {
                foreach (var style in filterStyles)
                {
                    DrawStylePanel(style);
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawStylePanel(BaseStyleDatabase style)
        {
            EditorGUILayout.BeginVertical("Panel");
            GUILayout.Label(style.name);
            foreach (var styleName in style.StyleNames)
            {
                if (GUILayout.Button(styleName))
                {
                    foreach (var gameObject in Selection.gameObjects)
                    {
                        style.Apply(styleName, gameObject);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}