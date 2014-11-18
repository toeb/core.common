using System;
using System.Runtime.InteropServices;

namespace Core
{
  public static class Ntfs
  {
    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool CreateHardLink(
    string lpFileName,
    string lpExistingFileName,
    IntPtr lpSecurityAttributes
    );
  }
}
