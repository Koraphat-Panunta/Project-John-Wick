using UnityEngine;
using UnityEngine.UI;

public class GameOverUICanvas : MonoBehaviour
{
    [SerializeField] Animator animator;

    public Button gameOverRestartButton;
    public Button gameOverExitButton;

    public void PlayFadeInGameOverCanvas() => animator.Play("GameOverAnimationUIFadeIn");
    public void PlayFadeOutGameOverCanvas() => animator.Play("GameOverAnimationUIFadeOut");
}
