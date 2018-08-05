using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity;
using Unity.Resolution;

namespace Abstractions.CuFileIo
{
  public class CuDirOnDiskHandler : ICuDirHandlerWithHost
  {
    public ICuDir Host { get; set; }
    public IEnumerable<ICuFile> GetFiles()
    {
      DirectoryInfo di = new DirectoryInfo(Host.Path);
      foreach (var file in di.EnumerateFiles())
      {
        yield return Host.Container.ResolveICuFile( file.FullName);
      }
    }

    public IEnumerable<ICuFile> GetFiles(string pattern)
    {
      DirectoryInfo di = new DirectoryInfo(Host.Path);
      foreach (var file in di.EnumerateFiles(pattern))
      {
        yield return Host.Container.ResolveICuFile(file.FullName);
      }
    }

    public void DeleteFiles(string pattern)
    {
      foreach (var file in Directory.GetFiles(pattern))
      {
        Directory.Delete(file);
      }
    }

    public void Create()
    {
      Directory.CreateDirectory(Host.Path);
    }

    public void DeleteFiles()
    {
      //Directory.Delete(Host);
    }

    public bool HasDir()
    {
      return Directory.Exists(Host.Path);
    }

    public IEnumerable<CuDir> GetCurrentBakDirectories()
    {
      var dirs = Directory.GetDirectories(Host.Path, "Bak*");
      foreach (CuDir subDir in dirs)
      {
        yield return subDir;
      }
    }

    public void Move(CuFile newFile)
    {
      File.Move(Host.Path, newFile);
    }
  }
}
