using UnityEngine;

namespace Rascar.Toolbox.Debugging
{
    /// <summary>
    /// Shows application version on start in console and on GUI.
    /// </summary>
    public class VersionNumber : MonoBehaviour
    {
        [SerializeField] private float _showVersionForSeconds = -1;
        [SerializeField] private bool _itMustAddCompanyName = true;

        private string _versionInformation;
        private Rect _position = new(0, 0, 300, 20);

        public string Version
        {
            get
            {
                if (_versionInformation == null)
                {
                    if (_itMustAddCompanyName)
                    {
                        _versionInformation = $"{Application.companyName} - {Application.productName} - {Application.version}";
                    }
                    else
                    {
                        _versionInformation = $"{Application.productName} - {Application.version}";
                    }
                }

                return _versionInformation;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this);

            Debug.Log(string.Format("Currently running version is {0}", Version));

            if (_showVersionForSeconds >= 0f)
            {
                Destroy(this, _showVersionForSeconds);
            }

            _position.x = 10f;
            _position.y = Screen.height - _position.height - 10f;
        }

        private void OnGUI()
        {
            if (_showVersionForSeconds == 0f)
            {
                return;
            }

            GUI.contentColor = Color.gray;
            GUI.Label(_position, Version);
        }
    }
}
