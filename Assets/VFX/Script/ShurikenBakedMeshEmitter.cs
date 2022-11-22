using UnityEngine;

public class ShurikenBakedMeshEmitter : MonoBehaviour
{
	[SerializeField] private SkinnedMeshRenderer skin = default;
	[SerializeField] private float coolDown = 0.5f;

	public SkinnedMeshRenderer Skin
	{
		set => skin = value;
	}

	public bool Emit 
	{ 
		get => emission.enabled;
		set => emission.enabled = value;
	}

	private Mesh baked = default;
	private ParticleSystem particle = default;
	private ParticleSystem.EmissionModule emission;
	private ParticleSystemRenderer render = default;
	private float interval = 0;

	private void Awake()
	{
        particle = GetComponent<ParticleSystem>();
        render = GetComponent<ParticleSystemRenderer>();
		emission = particle.emission;
    }

	void LateUpdate()
	{
		if (emission.enabled)
		{
			interval -= Time.deltaTime;
			
			if (interval < 0)
			{
                baked = new Mesh();
                skin.BakeMesh(baked);
                render.mesh = baked;
				
				interval = coolDown;
			}
			
		}
		else
		{
			interval = coolDown;
		}
	}
}