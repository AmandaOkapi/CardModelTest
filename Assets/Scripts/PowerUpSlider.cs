using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSlider : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    [SerializeField] private GameObject PowerUpActivator;
    [SerializeField] private Image PowerUpIcon;

    [SerializeField] private int fillSpeed;

    private void Start(){
        PowerUpActivator.SetActive(false);
    }
    public void SetMaxPoints(int maxPoints){
        slider.maxValue = maxPoints;
        slider.value =0;
        fill.color = gradient.Evaluate(1f);
    }   

    public void ResetSlider(PowerUp powerUp){
        slider.value =0;
        fill.color = gradient.Evaluate(0);
        PowerUpIcon.sprite= powerUp.icon;
    }
    public void SetSliderValue(float value){
        StartCoroutine(fillUpSlider(value));
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if(slider.value == slider.maxValue){
            PowerUpActivator.SetActive(true);
        }
    }

    private IEnumerator fillUpSlider(float target){
        while(slider.value< target){
            slider.value+=(fillSpeed)*Time.deltaTime;
            yield return null;
        }
    }
    public void StartDepleteSlider(float time){
        PowerUpActivator.SetActive(true);
        StartCoroutine(DepleteSlider(time));
    }

    private IEnumerator DepleteSlider(float time){
        float spd = slider.maxValue / time;
        while(slider.value> 0 ){
            slider.value-=(spd);
            yield return null;
        }
        yield return null;

    }

    public void StartWaitForPowerUpToEndAndThenReset(View view , PowerUp powerUp){
        StartCoroutine(WaitForPowerUpToEndAndThenReset( view , powerUp));
    }

    private IEnumerator WaitForPowerUpToEndAndThenReset(View view , PowerUp powerUp){
        while(view.IsFrenzy() || view.IsPowerUpPlaying()){
            yield return null;
        }
        ResetSlider(powerUp);
    }
}
