using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    public Image fadeImage; // 페이드 이미지를 여기에 연결합니다.
    public float fadeDuration = 1f; // 페이드 지속 시간
    public string nextSceneName; // 변경할 씬 이름
    private bool isAnimationFinished = false; // 애니메이션 종료 여부 확인

    private void Update()
    {
        if (isAnimationFinished)
        {
            StartCoroutine(FadeOut());
            isAnimationFinished = false; // 페이드 아웃이 시작되었으므로 true를 한 번만 실행
        }
    }

    // 애니메이션이 끝났을 때 호출하는 함수
    public void OnAnimationFinished()
    {
        isAnimationFinished = true; // 애니메이션이 끝났음을 표시
    }

    IEnumerator FadeOut()
    {
        // 페이드 아웃
        float timeElapsed = 0f;
        Color c = fadeImage.color;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, timeElapsed / fadeDuration); // 투명도 변화
            fadeImage.color = c;
            yield return null;
        }

        // 씬 변경
        SceneManager.LoadScene(nextSceneName);
    }
}
