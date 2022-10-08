
using Vanara.PInvoke;
using Vanara.Extensions;
using static Vanara.PInvoke.User32;
using static Vanara.PInvoke.Gdi32;
using static Vanara.PInvoke.Kernel32;
using static Vanara.PInvoke.Shell32;

namespace PowerCFG.Models
{
    public class IconExtractor
    {
        #region Constants
        // Resource types for EnumResourceNames().
        private static readonly IntPtr RT_GROUP_ICON = (IntPtr)14;
        #endregion

        #region Fields
        private Dictionary<int, Icon> iconData = new Dictionary<int, Icon>();
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the full path of the associated file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the count of the icons in the associated file.
        /// </summary>
        public int Count => iconData.Count;

        #endregion

        /// <summary>
        /// Initializes a new instance of the IconExtractor class from the specified file name.
        /// </summary>
        /// <param name="fileName">The file to extract icons from.</param>
        public IconExtractor(string fileName)
        {
            FileName = fileName;
            try
            {
                if (LoadLibraryEx(fileName, LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE) is SafeHINSTANCE hModule)
                {
                    foreach (var name in EnumResourceNamesEx(hModule, RT_GROUP_ICON))
                    {
                        SafeHICON hIcon = LoadIcon(hModule, name);
                        Icon icon = (Icon)Icon.FromHandle(hIcon.DangerousGetHandle()).Clone();
                        iconData.Add(name.id, icon);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Extracts an icon from the file.
        /// </summary>
        /// <param name="index">Zero based index of the icon to be extracted.</param>
        /// <returns>A System.Drawing.Icon object.</returns>
        /// <remarks>Always returns new copy of the Icon. It should be disposed by the user.</remarks>
        public Icon GetIcon(int id)
        {
            if (iconData.ContainsKey(id))
            {
                return iconData[id];
            }

            return null;
        }

        public Icon this[int id]
        {
            get => GetIcon(id);
        }

        public bool ContainsId(int id)
        {
            return iconData.ContainsKey(id);
        }

        /// <summary>
        /// Extracts all the icons from the file.
        /// </summary>
        /// <returns>An array of System.Drawing.Icon objects.</returns>
        /// <remarks>Always returns new copies of the Icons. They should be disposed by the user.</remarks>
        public Icon[] GetAllIcons()
        {
            return iconData.Values.ToArray();
        }

        public int[] GetAllIds()
        {
            return iconData.Keys.ToArray();
        }


    }
}
