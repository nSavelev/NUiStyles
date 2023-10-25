using System.Collections.Generic;
using UnityEngine;

namespace UI.Styles.Editor
{
    [CreateAssetMenu(fileName = "ProjectUiStyles", menuName = "N Ui/StylesHolder", order = 0)]
    public class ProjectStyles : ScriptableObject
    {
        [field:SerializeField]
        public List<BaseStyleDatabase> Styles { get; private set; }
    }
}