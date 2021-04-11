using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    private Animator anime;




    private void Awake()
    {
        anime = GetComponent<Animator>();

    }

    public void Walk(bool walk)
    {
        anime.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }

    public void Run(bool run)
    {
        anime.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    public void Attack()
    {
        anime.SetTrigger(AnimationTags.ATTACK_TRIGGER);
    }

    public void Dead()
    {
        anime.SetTrigger(AnimationTags.DEAD_TRIGGER);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}// end of class

















