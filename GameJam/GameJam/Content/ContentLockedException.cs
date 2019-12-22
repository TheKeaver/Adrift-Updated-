using System;

namespace GameJam.Content
{
    /// <summary>
    /// An exception thrown when the LockingContentManager is locked.
    /// </summary>
    public class ContentLockedException : Exception
    {
        public ContentLockedException() : base("ContentManager is locked. Did you remember to cache the content in `GameContent.cs`?")
        {
        }
    }
}
