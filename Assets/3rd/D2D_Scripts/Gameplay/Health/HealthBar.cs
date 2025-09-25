using UnityEngine;
using UnityEngine.UI;

namespace D2D.Gameplay
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Health targetHealth;
		[SerializeField] private Gradient gradient;
		[SerializeField] private Image fill;
		
		private Slider slider;

		private void OnEnable()
		{
			slider = GetComponent<Slider>();
			targetHealth.PointsChanged += UpdateSlider;
		}

		private void Start()
		{
			InitSlider();
		}

		private void OnDisable()
		{
			targetHealth.PointsChanged -= UpdateSlider;
		}

		private void InitSlider()
		{
			slider.maxValue = targetHealth.MaxPoints;
			slider.value = targetHealth.MaxPoints;

			fill.color = gradient.Evaluate(1f);
		}

		private void UpdateSlider()
		{
			slider.value = targetHealth.CurrentPoints;
			slider.maxValue = targetHealth.MaxPoints;
			
			fill.color = gradient.Evaluate(slider.normalizedValue);
		}
	}
}
