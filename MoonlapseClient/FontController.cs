using System;
using SadConsole;
namespace MoonlapseClient
{
    public class FontController
    {
        public Font TextFont { get; private set; }
        public Font GameFont { get; private set; }

        public void Init()
        {
            var fontMaster = Global.LoadFont("Assets/ibm_ext.font");
            TextFont = fontMaster.GetFont(Font.FontSizes.One);
            Global.FontDefault = TextFont;

            // loading and setting game font to 1 bit tileset
            fontMaster = Global.LoadFont("Assets/1bit.font");
            GameFont = fontMaster.GetFont(Font.FontSizes.Two);
            Global.FontDefault = GameFont;
        }
    }
}
