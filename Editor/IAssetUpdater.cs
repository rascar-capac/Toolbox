using UnityEngine;

namespace Rascar.Toolbox.Editor
{
    public interface IAssetUpdater
    {
        public void Prepare();
        public bool TryUpdateAsset(Object asset);
        public void Finish();
    }
}
