using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour {

	[Header("Set in Inspector")]
    public AudioClip sound;
	public AnimationCurve volume;
	public float minMagnitude;
    // Use this for initialization
    void OnCollisionEnter ( Collision other) {
        if (other.relativeVelocity.magnitude < minMagnitude)
            return;
		if (int.TryParse(this.gameObject.name, out int numThisObject))
        {
			if (int.TryParse(other.gameObject.name, out int numCollisionObject))
            {
				if (numThisObject > numCollisionObject)
                {
					AudioSource.PlayClipAtPoint(sound, other.contacts[0].point, volume.Evaluate(other.relativeVelocity.magnitude));
				}
            }
			return;
		}
		AudioSource.PlayClipAtPoint(sound, other.contacts[0].point,  volume.Evaluate(other.relativeVelocity.magnitude));
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}