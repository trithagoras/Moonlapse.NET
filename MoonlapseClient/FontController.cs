using System;
using SadConsole;
namespace MoonlapseClient
{
    public static class FontController
    {
        public static Font TextFont { get; private set; }
        public static Font GameFont { get; private set; }

        public static void Init()
        {
            var fontMaster = Global.LoadFont("Assets/ibm_ext.font");
            TextFont = fontMaster.GetFont(Font.FontSizes.One);
            Global.FontDefault = TextFont;

            // loading and setting game font to 1 bit tileset
            fontMaster = Global.LoadFont("Assets/colored_packed.font");
            GameFont = fontMaster.GetFont(Font.FontSizes.Two);
            Global.FontDefault = GameFont;
        }
    }
}
