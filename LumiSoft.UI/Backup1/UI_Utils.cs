using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LumiSoft.UI
{
    /// <summary>
    /// UI utility methods.
    /// </summary>
    public class UI_Utils
    {
        #region method ParseFont

        /// <summary>
        /// Parses font from the specified string value.
        /// </summary>
        /// <param name="value">Font string value.</param>
        /// <returns>Returns parsed font.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>value</b> is null reference.</exception>
        /// <exception cref="ArgumentException">Is raised when any of the arguments has invalid value.</exception>
        public static Font ParseFont(string value)
        {
            if(value == null){
                throw new ArgumentNullException("value");
            }
            if(value == ""){
                throw new ArgumentException("Argument 'value' value must be specified.");
            }

            string[] name_size_style = value.Split(',');
            if(name_size_style.Length != 3){
                throw new ArgumentException("Invalid argument 'value' value.");
            }

            return new Font(
                name_size_style[0],
                float.Parse(name_size_style[1],System.Globalization.NumberFormatInfo.InvariantInfo),
                (FontStyle)Enum.Parse(typeof(FontStyle),name_size_style[2])
            );
        }

        #endregion

        #region method FontToString

        /// <summary>
        /// Converts font to string value.
        /// </summary>
        /// <param name="font">Font value.</param>
        /// <returns>Returns font as string value.</returns>
        /// <exception cref="ArgumentNullException">Is raised when <b>font</b> is null reference.</exception>
        public static string FontToString(Font font)
        {
            if(font == null){
                throw new ArgumentNullException("font");
            }

            return font.FontFamily.Name + "," + font.Size.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," + font.Style.ToString();
        }

        #endregion
    }
}
