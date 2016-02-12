using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    /// <summary>
    /// ID motorů
    /// PP = pravý přední
    /// LP = levý přední
    /// PZ = pravý zadní
    /// LZ = levý zadní
    /// P = posun
    /// R = rotace kola
    /// Z = zdvih
    /// ZK = natočení nohy
    /// </summary>
    public enum MotorId {PP_P, LP_P, LZ_P, PZ_P, PP_R, LP_R, LZ_R, PZ_R, PP_Z, LP_Z, LZ_Z, PZ_Z, PP_ZK, LP_ZK, LZ_ZK, PZ_ZK};
}
