﻿using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
    public class PlayerVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;

        public void UpdateFootStep(bool state)
        {
            if (state)
            {
                footStep.Play();
            }
            else
            {
                footStep.Stop();
            }
        }
    }
}