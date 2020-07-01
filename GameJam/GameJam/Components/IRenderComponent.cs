using GameJam.Common;

namespace GameJam.Components
{
    interface IRenderComponent
    {
        bool IsHidden();
        BoundingRect GetAABB(float scale);
        byte GetRenderGroup();
    }
}
