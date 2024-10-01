using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Butcher_TA
{
    public class Gate : PointGiver
    {
        public override void ExtraAction() => Destroy(transform.parent.gameObject);
    }
}