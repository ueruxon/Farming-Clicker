using System.Collections;
using UnityEngine;

namespace Game.Scripts.Common
{
    public interface ICoroutineRunner
    { 
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopAllCoroutines();
    }
}