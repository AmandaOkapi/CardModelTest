using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSlider : MonoBehaviour
{
    //cursed code ahead proceed at your own risk
    public Slider slider;
    public Image fill;
    public Gradient gradient;

    [SerializeField] private GameObject PowerUpActivator;
    [SerializeField] private Image PowerUpIcon;

    [SerializeField] private int fillSpeed;
    [SerializeField] private float sliderNoMatch, sliderYesMatch;

    private float mySliderValue;
    private PowerUp nextPowerUp;
    [SerializeField] private List<PowerUp> powerUps;
    public void SetNextPowerUp(PowerUp powerUp){nextPowerUp = powerUp;}
    private Coroutine fillCoroutine; 

    //for testing :>
    public PowerUp revealRow, revealCol, frenzy;
    [SerializeField] private PowerUp startingPowerUp;

    private PowerUp currentPowerUp;
    private void Start(){
        PowerUpActivator.SetActive(false);
        AttachEvents();

        currentPowerUp = startingPowerUp;
        ResetSlider(currentPowerUp);
    }    
    private void AttachEvents()
    {
        EventManager.MatchFoundEvent +=MatchFound;
        //EventManager.WallDestroyed +=WallsDestoryed;
        EventManager.MatchFailed +=MatchFailed;   
    }

    void OnDisable()
    {
        EventManager.MatchFoundEvent -= MatchFound;
        //EventManager.WallDestroyed -= WallsDestoryed;
        EventManager.MatchFailed -= MatchFailed;   
    }

    private void MatchFound(int id1){
        if(!currentPowerUp.isPlaying){
            IncreaseSliderValue(sliderYesMatch);
        }
    }

    
    private void MatchFailed(int id1, int id2){
        if(!currentPowerUp.isPlaying){
            IncreaseSliderValue(sliderNoMatch);
        }
    }
    public void SetMaxPoints(int maxPoints){
        slider.maxValue = maxPoints;
        slider.value =0;
        fill.color = gradient.Evaluate(1f);
    }   

    public void ResetSlider(PowerUp powerUp){
        Debug.Log("resetting");
        slider.value =0;
        mySliderValue=0;
        fill.color = gradient.Evaluate(0);
        PowerUpIcon.sprite= powerUp.icon;
    }
    public void IncreaseSliderValue(float increaseAmount){
        mySliderValue = Mathf.Clamp(mySliderValue+increaseAmount, 0f, slider.maxValue);
        SetSliderValue(mySliderValue);        
    
    }
    public void SetSliderValue(float value){
        // Stop the currently running coroutine if it exists
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        // Start a new coroutine 
        fillCoroutine = StartCoroutine(FillUpSlider(value));
        fill.color = gradient.Evaluate(slider.normalizedValue);

    }

    public void PlayCurrentPowerUp(Controller controller){
        float invokeTime =(controller.cardsFlipped >0)? CardMono.Time_To_Flip +0.5f :0.5f;
        currentPowerUp?.Activate(controller.view, controller.model, invokeTime);
    }
    public void PlayDebugPowerUp(string powerUpName ){        
        PowerUp powerUpToActivate = null;

        switch (powerUpName.ToLower())
        {
            case "revealrow":
                powerUpToActivate = revealRow;
                break;
            case "revealcol":
                powerUpToActivate = revealCol;
                break;
            case "frenzy":
                powerUpToActivate = frenzy;
                break;
            default:
                Debug.LogError("Unknown power-up name: " + powerUpName);
                return;
        }
        if (powerUpToActivate != null)
        {
            Controller controller = GameObject.Find("Controller").GetComponent<Controller>();
            float invokeTime =(controller.cardsFlipped >0)? CardMono.Time_To_Flip +0.5f :0.5f;
            powerUpToActivate.Activate(controller.view, controller.model, invokeTime);
        }
    }
    private IEnumerator FillUpSlider(float target){
        while(slider.value< target ){
            Debug.Log("filling");
            slider.value+=(fillSpeed)*Time.deltaTime;
            yield return null;
        }          
        if(slider.value == slider.maxValue){
                Debug.Log("ACTIVATING");
                PowerUpActivator.SetActive(true);
            }
        Debug.Log("Goodbye from fill Coroute");
    }
    public void StartDepleteSlider(float time){
        PowerUpActivator.SetActive(false);
        mySliderValue=0;
        currentPowerUp.Deactivate();
        StartCoroutine(DepleteSlider(time));
    }

    private PowerUp ChooseNextPowerUp(){
        return powerUps[UnityEngine.Random.Range(0, powerUps.Count)];
    }


    private IEnumerator DepleteSlider(float time){
        Debug.Log("start depleteing for "+time);
        float spd = slider.maxValue/time;
        slider.value =slider.maxValue;
        while(slider.value > 0.1 ){
            Debug.Log("depleteing");
            slider.value -=spd* Time.deltaTime;
                        // Clamp the slider value to prevent it from going below 0
            if (slider.value < 0){
                slider.value = 0;
            }
            yield return null;
        }        
        currentPowerUp = ChooseNextPowerUp();
        ResetSlider(currentPowerUp);
    }




}
