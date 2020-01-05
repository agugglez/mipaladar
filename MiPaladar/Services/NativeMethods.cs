using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;


namespace MiPaladar.Services
{
    public class NativeMethods
    {
        // Use DllImport to import the Win32 MessageBox function.

        [DllImport(@"libraries\qdll.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern int _qdll(string command_path, int chandle, int context, int callBackProc);

        // Need this DllImport statement to reset the floating point register below
        [DllImport("msvcr71.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _controlfp(int n, int mask);

    }
}
