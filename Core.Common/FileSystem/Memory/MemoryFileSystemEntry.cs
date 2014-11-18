using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FileSystem.Memory
{

  public class MemoryFileSystemEntry
  {
    public MemoryFileSystemEntry()
    {
      UpdateCacheKey();
      CreationTime = DateTime.Now;
      LastAccessTime = DateTime.Now;
      LastWriteTime = DateTime.Now;
      Data = new byte[0];
    }
    public string CacheKey { get; set; }
    public void UpdateCacheKey()
    {
      CacheKey = Guid.NewGuid().ToString();
    }
    private byte[] data;
    public byte[] Data
    {
      get
      {
        data = data ?? new byte[0];
        LastAccessTime = DateTime.Now;
        return data;
      }
      set
      {
        if (data == value) return;
        data = value ?? null;
        LastWriteTime = DateTime.Now;
        UpdateCacheKey();
      }
    }
    public DateTime CreationTime { get; set; }
    public DateTime LastWriteTime { get; set; }
    public DateTime LastAccessTime { get; set; }

    public bool IsDirectory { get; set; }

    internal void Touch()
    {
      LastAccessTime = DateTime.Now;
      LastWriteTime = DateTime.Now;
    }
  }
}
