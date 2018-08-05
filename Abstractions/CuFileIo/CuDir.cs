using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Core;
using Abstractions.Handler;
using Unity.Resolution;
using Unity;
using Microsoft.Practices.Unity;
using Unity.Attributes;

namespace Abstractions.CuFileIo
{
  public class CuDir : CuBase, ICuDir
  {
    [Dependency]
    public Func<ICuDirHandlerWithHost> _handlerFactory
    {
      set => getHandler = new Lazy<ICuDirHandlerWithHost>(() =>
      {
        var instance = value();
        instance.Host = this; //TODO
        return instance;
      });
    }

    private Lazy<ICuDirHandlerWithHost> getHandler;
    public ICuDirHandler Handler
    {
      get
      {
        return getHandler.Value;
      }
    }

    private readonly String _path;
    public String Path
    {
      get { return _path; }
    }

    public CuDir Copy()
    {
      return new CuDir(Path); // wait need new handlers also !! need factory to create this??!!
    }
    private Lazy<String> _cacheLastPart;
    private Lazy<ICuDir> _cacheParentPath;
    public static char Delimiter = System.IO.Path.DirectorySeparatorChar;

    public CuDir(string path)
    {
      if (path[path.Length - 1] == Delimiter) throw new Exception("CuDir should not end with delimiter");
      if (path.IndexOf(Delimiter) == -1) throw new Exception("Path delimiter expected");
      _path = path;
      _cacheLastPart = new Lazy<String>(() =>
      {
        var idx = Path.LastIndexOf( Delimiter + "", StringComparison.CurrentCulture);
        if (idx == -1) throw new Exception("Invalid CuDir");
        return Path.Substring(idx + 1);
      });

      _cacheParentPath = new Lazy<ICuDir>(() =>
      {
        var parent = Path.Substring(0, Path.Length - LastPart.Length - 1);
        return new CuDir(parent);
        //var po = new ParameterOverride(ConstrPath, parent);
        //return (CuDir)Container.Resolve<ICuDir>(po);
      });

    }

    public String LastPart
    {
      get
      {
        return _cacheLastPart.Value;
      }
    }

    public String LastPartAsUpper
    {
      get
      {
        return _cacheLastPart.Value.ToUpper();
      }
    }

    public ICuDir Parent
    {
      get { return _cacheParentPath.Value; }
    }

    public override string ToString()
    {
      return Path;
    }

    public static CuDir operator +(CuDir c1, String c2)
    {
      return c1.Path + Delimiter + c2;
    }

    public static CuFile operator -(CuDir c1, String c2)
    {
      return c1.Path + Delimiter + c2;
    }

    public static implicit operator CuDir(string path)
    {
      return new CuDir(path);
    }

    public static implicit operator string(CuDir p)
    {
      return p.Path;
    }

  }
}

