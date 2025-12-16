using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUICanvas : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] public Button continueButton;
    [SerializeField] public Button restartButton;
    public void PlayFadeIn() => animator.Play("MissionCompleteAnimationUIFadeIn");
    public void PlayFadeOut() => animator.Play("MissionCompleteAnimationUIFadeOut");
}
