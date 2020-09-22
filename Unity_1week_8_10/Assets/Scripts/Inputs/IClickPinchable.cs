using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

namespace Inputs{
    public interface IClickPinchable{
        IReadOnlyReactiveProperty<bool> rClicked {get;}
    }
}
