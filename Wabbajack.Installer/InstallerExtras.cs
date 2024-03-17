using System.Collections.Generic;
using System.Threading.Tasks;
using Wabbajack.DTOs;
using Wabbajack.Hashing.xxHash64;
using Wabbajack.Paths;

namespace Wabbajack.Installer;

public class InstallerExtras
{
    public readonly Task<Dictionary<Game, Dictionary<Hash, AbsolutePath>>> HashFallbacks;

    public InstallerExtras(Task<Dictionary<Game, Dictionary<Hash, AbsolutePath>>> hashFallbacks)
    {
        HashFallbacks = hashFallbacks;
    }

}