using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBlocksRow : MonoBehaviour
{
    public ExplosionBlock[] explosionBlocks;
    public float waitBetween;

    private Coroutine _explodeInRow;

    /// <summary>
    /// Explode blocks.
    /// </summary>
    public void ExplodeBlocks()
    {
        if (_explodeInRow == null)
        {
            StartCoroutine(ExplodeBlocksRoutine());
        }
    }
       
    /// <summary>
    /// Explode blocks in a row corotuine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ExplodeBlocksRoutine()
    {
        foreach (ExplosionBlock block in explosionBlocks)
        {
            block.Explode();
            yield return new WaitForSeconds(waitBetween);
        }

        _explodeInRow = null;
    }
    
}
