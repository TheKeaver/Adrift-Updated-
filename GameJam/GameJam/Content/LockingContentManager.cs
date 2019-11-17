using System;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Content
{
    /// <summary>
    /// Wrapper around ContentManager that prevents loading when locked.
    /// </summary>
    public class LockingContentManager : ContentManager
    {
        public bool Locked
        {
            get;
            set;
        }

        public LockingContentManager(IServiceProvider services) : base(services)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0133:Parameter name differs in base declaration", Justification = "Functional behavior difference compared to XNA.ContentManager. ContentManager loads using CVar aliases instead of asset paths directly.")]
        public override T Load<T>(string cvarAssetName)
        {
            string assetName;
            try
            {
                assetName = CVars.Get<string>(cvarAssetName);
            } catch (Exception)
            {
                throw new Exception(string.Format("Can not load content (CVar does not exist): `${0}`.", cvarAssetName));
            }

            if (Locked && !LoadedAssets.ContainsKey(assetName))
            {
                throw new ContentLockedException();
            }

            return base.Load<T>(assetName);
        }

    }
}
