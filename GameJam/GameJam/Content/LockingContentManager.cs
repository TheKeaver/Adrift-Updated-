using System;
using GameJam.Content;
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

        public override T Load<T>(string assetName)
        {
            if (Locked && !LoadedAssets.ContainsKey(assetName))
            {
                throw new ContentLockedException();
            }

            return base.Load<T>(assetName);
        }

    }
}
