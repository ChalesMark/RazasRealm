using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.Entities
{
    class AttachedParticleEffect : MonoBehaviour
    {
        public ParticleSystem ps;
        ParticleSystem internalPS;

        private void Start()
        {
            if (ps)
                internalPS = Instantiate(ps);
        }


        // Update
        // Runs every frame
        void Update()
        {
            if (internalPS)
                internalPS.transform.position = this.transform.position;
        }
    }
}