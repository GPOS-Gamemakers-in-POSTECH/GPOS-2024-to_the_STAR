using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private int currentAnimationIndex = 0;
    public string[] animationNames; // 애니메이션 클립 이름 배열
    public string nextSceneName; // 다음 씬 이름
    public SceneFade scenefade; // FadeOutAfterAnimation 스크립트

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            PlayNextAnimation();
        }
    }

    void PlayNextAnimation()
    {
        if (currentAnimationIndex < animationNames.Length - 1)
        {
            animator.SetTrigger("Click"); // 애니메이션 전환 트리거
            animator.Play(animationNames[currentAnimationIndex]);
            currentAnimationIndex++;
        }
        else
        {
            scenefade.OnAnimationFinished();
        }
    }
}