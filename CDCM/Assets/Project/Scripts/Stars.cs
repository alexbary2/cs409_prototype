using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {
    
    //modified code from this video: https://www.youtube.com/watch?v=YuPEmRXtwIg
    private Transform thisTrans;
    private ParticleSystem.Particle[] stars;
    private float starDistSqr;
    private float starClipDistSqr;

    public Color starColor;
    public int maxStars = 600;
    public float starSize = .35f;
    public float distanceFromUser = 50f;
    public float cameraClipDistance = 15f;


	// Use this for initialization
	void Start () {
        thisTrans = GetComponent<Transform>();
        starDistSqr = distanceFromUser * distanceFromUser;
        starClipDistSqr = cameraClipDistance * cameraClipDistance;

	}
	
    private void CreateParticles()
    {
        stars = new ParticleSystem.Particle[maxStars];

        for(int i = 0; i < maxStars; i++)
        {
            stars[i].position = Random.insideUnitSphere * distanceFromUser + thisTrans.position;
            stars[i].color = new Color(starColor.r, starColor.g, starColor.b, starColor.a);
            stars[i].size = starSize;

        }

    }
	// Update is called once per frame
	void Update () {
		if(stars == null)
        {
            CreateParticles();
        }

        for (int i = 0; i < maxStars; i++)
        {
            if((stars[i].position - thisTrans.position).sqrMagnitude > starDistSqr)
            {
                stars[i].position = Random.insideUnitSphere.normalized * distanceFromUser + thisTrans.position;
            }
            if ((stars[i].position - thisTrans.position).sqrMagnitude <= starClipDistSqr)
            {
                float percent = (stars[i].position - thisTrans.position).sqrMagnitude / starClipDistSqr;
                stars[i].color = new Color(1, 1, 1, percent);
                stars[i].size = percent * starSize;
            }
        }
        GetComponent<ParticleSystem>().SetParticles(stars, stars.Length);

    }
}
