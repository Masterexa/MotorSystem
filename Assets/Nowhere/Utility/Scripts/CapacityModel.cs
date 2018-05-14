using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NowhereUnity.Utility{

    /// <summary>
    ///　能力を示すクラス
    /// </summary>
    public abstract class CapacityModel {

        #region Instance
            #region Methods
                public CapacityModel() {
                    Reset();
                }

                /// <summary>
                /// 値の初期化
                /// </summary>
                public abstract void Reset();
            #endregion
        #endregion
    }
}
