using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace D2D.Gameplay
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Health targetHealth;
		[SerializeField] private Gradient gradient;
		[SerializeField] private Image fill;
		[SerializeField] private float fadeDuration = .7f;
		[SerializeField] private float showDuration = 2f;
		[SerializeField] private Slider slider;
		
		private CanvasGroup canvasGroup;

		private Tween fadeTween;

		private void Awake()
		{
			slider = GetComponent<Slider>();
			canvasGroup = GetComponent<CanvasGroup>();
		}

		private void Start()
		{
			if (targetHealth != null)
            {
				InitSlider();
            }
		}

		private void OnDisable()
		{
			targetHealth.PointsChanged -= UpdateSlider;
		}

		private void InitSlider()
		{
			targetHealth.PointsChanged += UpdateSlider;

			slider.maxValue = targetHealth.MaxPoints;
			slider.value = targetHealth.MaxPoints;

			fill.color = gradient.Evaluate(1f);
		}

		private void UpdateSlider()
		{
			if (fadeTween != null)
            {
				fadeTween = fadeTween.KillTo0();
            }

			canvasGroup.DOFade(1, 0);
			fadeTween = canvasGroup.DOFade(0, fadeDuration).SetDelay(showDuration);

			slider.value = targetHealth.CurrentPoints;
			slider.maxValue = targetHealth.MaxPoints;
			
			fill.color = gradient.Evaluate(slider.normalizedValue);
		}

		public void SetHealth(Health health)
        {
			targetHealth = health;

			InitSlider();
        }
	}
}
