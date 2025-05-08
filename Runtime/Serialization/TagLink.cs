using UnityEngine;

namespace Rascar.Toolbox.Serialization
{
    [System.Serializable]
    public class TagLink
    {
        [SerializeField] private string _tagName;

        public string TagName => _tagName;

        public TagLink(string tagName)
        {
            _tagName = tagName;
        }
    }
}
