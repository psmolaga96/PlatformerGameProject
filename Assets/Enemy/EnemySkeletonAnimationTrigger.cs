using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTrigger : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger() { enemy.AnimationFinishTrigger(); }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadious);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }
    public void MakeTransparent() => StartCoroutine("FadeOut");
    IEnumerator FadeOut()
    {
        for (float i = 1f; i >- 0.05f; i-= 0.05f)
        {
            Color c = enemy.sr.material.color;
            c.a = i;
            enemy.sr.material.color = c;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
