using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class SpriteRendererExtensions
    {
        public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
