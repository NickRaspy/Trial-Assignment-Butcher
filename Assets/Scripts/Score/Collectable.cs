using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Butcher_TA
{
    public class Collectable : ScoreGiver
    {
        [SerializeField] private GameObject collectableObject;

        public override void ExtraAction() => Destroy(gameObject);

        private void Update()
        {
            collectableObject.transform.Rotate(0f, Time.deltaTime * 10f, 0f);
        }
    }
}
