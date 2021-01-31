using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFires : MonoBehaviour
{
    // private AnimationVariables gamePlaySignals;
    public bool burn = false;

    public List<Animator> animatorsToAffect;

    private IgnitionTarget thisIgniteTarget;
    public bool useList = false;
    public List<IgnitionTarget> ignitionTargets;

    [Header("Fire")]
    public FXParticlesSettings fireSettings;
    private IgnitionTarget[] fireTargetArray;
    private particleStatusFX[] litFires = new particleStatusFX[0];




    [System.Serializable]
    public class FXParticlesSettings
    {
        public bool lightUp = false;
        public bool lit = false;
        public bool extinguished = false;

        public ParticleSystemSimulationSpace simSpace = ParticleSystemSimulationSpace.World;
        public ParticleSystemMeshShapeType shapeEmittPoints = ParticleSystemMeshShapeType.Vertex;

        public Material particleMaterial;
        public int xSprite = 4;
        public int ySprite = 2;

        public float startLifeTime = 1f;
        public float startSize = .01f;


        public float emissionRate = 500f;
        public int maxParticles = 1000;

        public float gravity = -.5f;


        public FXParticlesSettings(bool play, bool started, bool stopped, ParticleSystemSimulationSpace sim, ParticleSystemMeshShapeType emitPoints, Material particleMat, int xTile, int yTile, float stLife, float stSize, float emissRt, int mxParticles,  float grav, string soundPath)
        {
            lightUp = play;
            lit = started;
            extinguished = stopped;

            simSpace = sim;
            shapeEmittPoints = emitPoints;
            particleMaterial = particleMat;
            xSprite = xTile;
            ySprite = yTile;

            startLifeTime = stLife;
            startSize = stSize;

            emissionRate = emissRt;
            maxParticles = mxParticles;

  

            gravity = grav;
        }
    }

    [System.Serializable]
    public class IgnitionTarget
    {
        public GameObject targetObject;
        public float scaleModifier = 1f;
        public float lifetimeModifier = 1f;

        public IgnitionTarget(GameObject tObj, float sm, float lm)
        {
            targetObject = tObj;
            scaleModifier = sm;
            lifetimeModifier = lm;
        }

    }

    public class particleStatusFX
    {
        public ParticleSystem statusPartices;

        public particleStatusFX(ParticleSystem pFX)
        {
            statusPartices = pFX;


        }


    }

    public class PFXAnimationModulator
    {
        public bool lit = false;
        public bool litComplete = false;
        public bool snuffedOut = false;
        public float animationModifier = 1;
        public List<Animator> animators;


        public PFXAnimationModulator(bool ls, bool lf, bool ef, float mod, List<Animator> anims)
        {
            lit = ls;
            litComplete = lf;
            snuffedOut = ef;
            animationModifier = mod;
            animators = anims;
        }

        public void ExecuteAnimationSpeedFX()
        {
            //Debug.Log("attempting ot adjust speed");
            if (lit == true && litComplete == false)
            {
                // Debug.Log("status effect is active, modifier needs application");
                foreach (Animator anim in animators)
                {
                    anim.speed = anim.speed * animationModifier;
                    //  Debug.Log("modifying animator speed is now" + anim.speed);
                }
                litComplete = true;
                snuffedOut = false;
                // Debug.Log("modification complete" + litComplete);
            }
            else if (lit == false && snuffedOut == false)
            {
                foreach (Animator anim in animators)
                {
                    anim.speed = 1;
                    //  Debug.Log("resetting animator speed is now" + anim.speed);
                }
                snuffedOut = true;
                litComplete = false;
                // Debug.Log("reset complete" + snuffedOut);
            }

        }

    }


    // Start is called before the first frame update
    private void OnEnable()
    {
      


    }

    void Start()
    {


        ///fire
        fireSettings.lightUp = false;
        fireSettings.lit = false;
        litFires = new particleStatusFX[ignitionTargets.Count];
        fireTargetArray = ignitionTargets.ToArray();


        thisIgniteTarget = new IgnitionTarget(this.gameObject, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        ManageElementalFX(burn, fireSettings, litFires, fireTargetArray);

    }

    //feed in appropriate bool from animvars
    void ManageElementalFX(bool burning, FXParticlesSettings fxSettings, particleStatusFX[] litParticleFX, IgnitionTarget[] fxTargetArray, float speedModifier = 1)
    {
        if (burning == true)
        {
            fxSettings.lightUp = true;
            fxSettings.extinguished = false;
        }
        else if (burning == false)
        {
            fxSettings.extinguished = true;
        }

        if (fxSettings.lightUp == true && fxSettings.lit == false && useList == true)
        {
            for (int i = 0; i < litParticleFX.Length; i++)
            {
                litParticleFX[i] = Ignite(fxTargetArray[i], fxSettings);

            }
            fxSettings.lit = true;

        }
        else if (fxSettings.lightUp == true && fxSettings.lit == false && useList == false)
        {
            litParticleFX = new particleStatusFX[1];
            litParticleFX[0] = Ignite(thisIgniteTarget, fxSettings);




            fxSettings.lit = true;
        }


        if (fxSettings.lit == true && fxSettings.extinguished == true && useList == true)
        {
            //Debug.Log("stopping fires");
            for (int i = 0; i < litParticleFX.Length; i++)
            {
                litParticleFX[i].statusPartices.Stop();
            }
        }
        else if (fxSettings.lit == true && fxSettings.extinguished == false && useList == true)
        {
            //   Debug.Log("lighting fires");
            for (int i = 0; i < litParticleFX.Length; i++)
            {

                litParticleFX[i].statusPartices.Play();



            }

        }
        else if (fxSettings.lit == true && fxSettings.extinguished == true && useList == false)
        {
            litParticleFX[0].statusPartices.Stop();

        }
        else if (fxSettings.lit == true && fxSettings.extinguished == false && useList == false)
        {

            litParticleFX[0].statusPartices.Play();



        }
    }

    particleStatusFX Ignite(IgnitionTarget igniteTarget, FXParticlesSettings fxSettings)
    {
        GameObject flameHolder = new GameObject("FX Holder");
        GameObject targetObj = igniteTarget.targetObject;
        flameHolder.transform.parent = igniteTarget.targetObject.transform;
        flameHolder.transform.localPosition = new Vector3(0, 0, 0);
        flameHolder.transform.localScale = new Vector3(1, 1, 1);
        ParticleSystem fire = flameHolder.AddComponent<ParticleSystem>();
        var fireSourceShape = fire.shape;


        if (targetObj.GetComponent<SkinnedMeshRenderer>() != null)
        {
            fireSourceShape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
            fireSourceShape.skinnedMeshRenderer = targetObj.GetComponent<SkinnedMeshRenderer>();

            Debug.Log("I have a skinned mesh renderer");
        }
        else if (targetObj.GetComponent<MeshRenderer>() != null)
        {
            fireSourceShape.shapeType = ParticleSystemShapeType.MeshRenderer;
            fireSourceShape.meshRenderer = targetObj.GetComponent<MeshRenderer>();
            Debug.Log("I have a mesh renderer");
        }
        else if (targetObj.GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            fireSourceShape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
            fireSourceShape.skinnedMeshRenderer = targetObj.GetComponentInChildren<SkinnedMeshRenderer>();
            Debug.Log("I have a skinned mesh renderer in children");
        }
        else if (targetObj.GetComponentInChildren<MeshRenderer>() != null)
        {
            fireSourceShape.shapeType = ParticleSystemShapeType.MeshRenderer;
            fireSourceShape.meshRenderer = targetObj.GetComponentInChildren<MeshRenderer>();
            Debug.Log("I have a mesh renderer in children");
        }

        fireSourceShape.useMeshColors = false;
        fireSourceShape.meshShapeType = fxSettings.shapeEmittPoints;














        ParticleSystemRenderer fireRenderer = fire.GetComponent<ParticleSystemRenderer>();
        fireRenderer.material = fxSettings.particleMaterial;






        var fireSpriteAnimation = fire.textureSheetAnimation;
        fireSpriteAnimation.enabled = true;
        fireSpriteAnimation.numTilesX = fxSettings.xSprite;
        fireSpriteAnimation.numTilesY = fxSettings.ySprite;





        ////emission
        var fireEmission = fire.GetComponent<ParticleSystem>().emission;

        fireEmission.rateOverTime = new ParticleSystem.MinMaxCurve(fxSettings.emissionRate);


        ///main module settings

        var fireMain = fire.main;
        //fireMain.duration = 1f;
        fireMain.startSize = fxSettings.startSize;
        fireMain.startSpeed = 0f;
        fireMain.startLifetime = fxSettings.startLifeTime * igniteTarget.lifetimeModifier;
        fireMain.maxParticles = fxSettings.maxParticles;
        fireMain.simulationSpace = fxSettings.simSpace;
        fireMain.gravityModifier = fxSettings.gravity;
        fireMain.scalingMode = ParticleSystemScalingMode.Hierarchy;




        /////audio
        ///






        //settings for built in unity audio sources
        /*
        AudioSource fireSound = igniteTarget.AddComponent<AudioSource>();
        fireSound.clip = burningSound;
        fireSound.outputAudioMixerGroup = fireSoundOutput;
        fireSound.volume = soundVolume;
        fireSound.minDistance = minDistance;
        fireSound.loop = true;
        fireSound.spatialBlend = 1f;
        fireSound.time = Random.Range(0, fireSound.clip.length);
        fireSound.Play();
        */


        particleStatusFX pFX;

        pFX = new particleStatusFX(fire);



        return pFX;

    }

   









    private void OnDisable()
    {

    }
}
