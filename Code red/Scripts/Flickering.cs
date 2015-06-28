using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System.Collections.Generic;

public class Flickering : MonoBehaviour {

    public enum FlickerStates { 
        Functional,
        FunctionalFlickering,
        BrokenFlickering,
        Rave,
        Alarm
    }
    public float FunctionalFlickerSpeed = 0.3f;
    public float FunctionalFlickerInterval = 0.8f;
    public float FunctionalFlickerDuration = 0.5f;
    public float FunctionalFlickerIntervalRange = 0.3f;
    public float FunctionalFlickerSpeedRange = 1f;
    public float FunctionalFlickerDurationRange = 0.2f;

    public float AlarmIntensity = 1f;
    public float AlarmSpeed = 0.3f;
    public float AlarmInterval = 0.8f;
    public float AlarmDuration = 0.5f;
    public Color AlarmColor = Color.red;


    public Material LightMap;

    public FlickerStates __CurrentFlickerState;
    public FlickerStates CurrentFlickerState {
    
        get{return __CurrentFlickerState;}
        set {__CurrentFlickerState = value; StateSwitched(); }
    }

    Light attachedLight;

    private float _originalIntensity;
    private Color _originalColor;

	// Use this for initialization
	void Start () {
        attachedLight = GetComponent<Light>();
        _originalIntensity = attachedLight.intensity;
        _originalColor = attachedLight.color;
        StateSwitched();

   
	}
	
	// Update is called once per frame
	void StateSwitched () {
        
        switch (CurrentFlickerState)
        {
            case FlickerStates.Functional:
                CancelStartCoroutine(Revert());
                break;
            case FlickerStates.FunctionalFlickering:
                CancelStartCoroutine(FunctionalFlickering());
                break;
            case FlickerStates.BrokenFlickering:
                break;
            case FlickerStates.Rave:
                break;
            case FlickerStates.Alarm:
                CancelStartCoroutine(Alarm());
                break;
            default:
                break;
        }
	}


    private Coroutine _currentCoroutine;
    private List<Tweener> _tweens = new List<Tweener>();


    private void CancelStartCoroutine(IEnumerator targetRoutine)
    {
        foreach (var tween in _tweens)
            tween.Kill();
        _tweens.Clear();

        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        
        _currentCoroutine = StartCoroutine(targetRoutine);
    }
    

    public IEnumerator Revert()
    {

        _tweens.Add(HOTween.To(attachedLight, FunctionalFlickerInterval, "intensity", _originalIntensity));
        attachedLight.color = Color.Lerp(attachedLight.color, _originalColor, 1f);

        yield return null;
    }

    public IEnumerator FunctionalFlickering()
    {
        while (true)
        {
            var duration = Random.Range(FunctionalFlickerDuration-FunctionalFlickerDurationRange, FunctionalFlickerDuration+FunctionalFlickerDurationRange);
            var speed = Random.Range(FunctionalFlickerSpeed-FunctionalFlickerSpeed, FunctionalFlickerSpeed+FunctionalFlickerSpeed);

            _tweens.Add(HOTween.To(attachedLight, speed, "intensity", 0));
            yield return new WaitForSeconds(duration);
            var interval = Random.Range(FunctionalFlickerInterval - FunctionalFlickerIntervalRange, FunctionalFlickerInterval + FunctionalFlickerIntervalRange);
            _tweens.Add(HOTween.To(attachedLight, speed, "intensity", _originalIntensity));
                
            yield return new WaitForSeconds(interval);
        }
    }


    public IEnumerator Alarm()
    {
        while (true)
        {

            attachedLight.color = Color.Lerp(attachedLight.color, AlarmColor, AlarmSpeed);

            _tweens.Add(HOTween.To(attachedLight, AlarmSpeed, "intensity", AlarmIntensity));
            
            yield return new WaitForSeconds(AlarmDuration);

            _tweens.Add(HOTween.To(attachedLight, AlarmSpeed, "intensity", 0));

            yield return new WaitForSeconds(AlarmInterval);
        }
    }
}
