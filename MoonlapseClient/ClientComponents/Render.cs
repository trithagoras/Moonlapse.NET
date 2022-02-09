using System;
using MoonlapseNetworking.Models.Components;
using Microsoft.Xna.Framework;

namespace MoonlapseClient.ClientComponents
{
    [HiddenComponent]
    public class Render : Component
    {
        public int GlyphIndex { get; set; }
        public Color Color { get; set; }
    }
}
