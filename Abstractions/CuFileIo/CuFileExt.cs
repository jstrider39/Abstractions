using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.CuFileIo
{
  public static class CuFileExt
  {

    //private CuFile Host { get; set; }
    public static IEnumerable<ICuFile> GetFiles(this CuDir cuDir)
    {
      foreach (var cuFile in cuDir.Handler.GetFiles())
      {
        yield return cuFile;
      }
    }

    public static IEnumerable<ICuFile> GetFiles(this CuDir cuDir, string pattern)
    {
      foreach (var cuFile in cuDir.Handler.GetFiles(pattern))
      {
        yield return cuFile;
      }
    }

    public static void DeleteFiles(this CuDir cuDir, string pattern)
    {
      cuDir.Handler.DeleteFiles(pattern);
    }

    public static void Create(this CuDir cuDir)
    {

      if (cuDir.Handler.HasDir()) throw new Exception("CuDir already exists " + cuDir.Path);
      cuDir.Handler.Create();
    }


    public static void BackUpSqlFiles(this CuDir cuDir)
    {
      BackUpFiles(cuDir, "*.sql");
    }

    public static void BackUpFiles(this CuDir cuDir, string pattern)
    {
      var list =  cuDir.Handler.GetCurrentBakDirectories().Select(x => x.LastPartAsUpper).ToList();
      var newBackUpFolderName = "";
      foreach (var i in Enumerable.Range(1, 100))
      {
        if (!list.Contains("BAK" + i))
        {
          newBackUpFolderName = "BAK" + i;
          break;
        }
      }
      if (newBackUpFolderName == "")
      {
        throw new Exception("Backup failed, so delete some backup folders --max backup folders limit reached");
      }

      CuDir backUpDir = cuDir.Container.ResolveICuDir(cuDir.Path) + newBackUpFolderName;
      backUpDir.Create();
      foreach (CuFile file in cuDir.GetFiles(pattern))
      {
        var newFiel = backUpDir - file.Name;
        Debug.WriteLine($"Move: {file} > {newFiel}");
        cuDir.Handler.Move(file, newFiel);
      }
    }





  }
}
