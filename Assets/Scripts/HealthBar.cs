using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private HealthSO _healthSO = default;

    private Slider _slider = default;

    public void InitializeHealthBar(HealthSO healthSO)
    {
        _healthSO = healthSO;
        _slider = GetComponent<Slider>();
        _slider.value = 1;
        _healthSO.resetHealth();
    }


    private void Update()
    {
        if (_healthSO != null)
        {
            float sliderValue = (float)_healthSO.GetHealth() / (float)_healthSO.GetStartingHealth();
            _slider.value = sliderValue;
        }
        
    }
}
