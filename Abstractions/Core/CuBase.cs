using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.CuFileIo;
using Abstractions.Handler;
using Unity;
using Unity.Attributes;
using Unity.Resolution;

namespace Abstractions.Core
{
  public interface ICuBase
  {
  }

  public abstract class CuBase : ICuBase, ICuSys
  {
    public static readonly String ContainPropStr = "Container";

    public CuUnityContainer Container { get; set; }
  }

  public class CuUnityContainer
  {
    public UnityContainer Container { get; set; }

    public ICuDir ResolveICuDir(String path)
    {
      return Container.Resolve<ICuDir>(new ParameterOverride("path", path));
    }
    public ICuFile ResolveICuFile(String path)
    {
      return Container.Resolve<ICuFile>(new ParameterOverride("path", path));
    }
  }
}

