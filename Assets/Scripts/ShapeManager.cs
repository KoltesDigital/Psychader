using UnityEngine;
using System.Collections.Generic;

public class ShapeManager : MonoBehaviour
{
	private class Block
	{
		public GameObject parent;
		public List<Shape> shapes;

		private float _ratio;
		public float ratio
		{
			get
			{
				return _ratio;
			}
			set
			{
				_ratio = value;

				foreach (Shape shape in shapes)
				{
					shape.SetRatio(_ratio);
				}
			}
		}

		public bool ended
		{
			get
			{
				return ratio >= 1.0f;
			}
		}

		public Block(GameObject _parent)
		{
			parent = _parent;
			shapes = new List<Shape>();
			_ratio = 0.0f;
		}

		public void AddShape(Shape shape)
		{
			shape.SetRatio(0.0f);
			shapes.Add(shape);
		}
	}

	public Transform spawnConfigurations;
	private TetrisRandomGenerator<Transform> spawnConfigurationRG = new TetrisRandomGenerator<Transform>(0.5f);
	private Transform currentSpawnConfiguration;

	public string shapeResourceDirectory = "Shapes";
	private TetrisRandomGenerator<GameObject> shapeRG = new TetrisRandomGenerator<GameObject>(0.5f);
	private GameObject currentShape;

	private TetrisRandomGenerator<float> angleRG = new TetrisRandomGenerator<float>(0.5f);
	private float currentAngle;

	public float ratioSpeed = 1.0f;
	private List<Block> spawnedBlocks = new List<Block>();

	public float spawnInterval = 1.0f;
	private float spawnCountdown = 0.0f;

	public int spawnMinSequence = 2;
	public int spawnMaxSequence = 6;
	private int spawnSequence = 0;

	public float hueMinIncrement = 0.1f;
	public float hueMaxIncrement = 0.3f;
	private float currentHue = 0.0f;

	public float minSaturation = 0.8f;
	public float maxSaturation = 1.0f;
	public float minValue = 0.8f;
	public float maxValue = 1.0f;

	void Start()
	{
		GameObject[] shapes = Resources.LoadAll<GameObject>(shapeResourceDirectory);
		if (shapes == null)
		{
			Debug.LogError("Wrong Shape Resource Directory");
		}

		foreach (Transform spawnConfiguration in spawnConfigurations)
		{
			spawnConfigurationRG.Add(spawnConfiguration);
		}
		spawnConfigurationRG.Shuffle();

		foreach (GameObject shape in shapes)
		{
			shapeRG.Add(shape);
		}
		shapeRG.Shuffle();

		angleRG.Add(0.0f);
		angleRG.Add(45.0f);
		angleRG.Add(90.0f);
		angleRG.Add(135.0f);

		UpdateSpawn();
	}
	
	void Update()
	{
		float dRatio = Time.deltaTime * ratioSpeed;

		for (int i = 0; i < spawnedBlocks.Count; ++i)
		{
			Block block = spawnedBlocks[i];
			block.ratio += dRatio;
			if (block.ended)
			{
				Destroy(block.parent);
				spawnedBlocks.RemoveAt(i);
				--i;
			}
		}

		spawnCountdown -= Time.deltaTime;
		UpdateSpawn();
	}

	void UpdateSpawn()
	{
		if (spawnCountdown <= 0.0f)
		{
			--spawnSequence;
			if (spawnSequence <= 0)
			{
				ChangeSequence();
				spawnSequence = Random.Range(spawnMinSequence, spawnMaxSequence + 1);
			}

			Spawn();
			spawnCountdown = spawnInterval;
		}
	}

	void ChangeSequence()
	{
		currentSpawnConfiguration = spawnConfigurationRG.Draw();
		currentShape = shapeRG.Draw();
		currentAngle = angleRG.Draw();
	}


	void Spawn()
	{
		currentHue = Mathf.Repeat(currentHue + Random.Range(hueMinIncrement, hueMaxIncrement), 1.0f);
		Color color = Random.ColorHSV(currentHue, currentHue, minSaturation, maxSaturation, minValue, maxValue);
		
		GameObject parent = new GameObject();
		parent.transform.SetParent(transform, true);
		Block block = new Block(parent);

		foreach (Transform spawnPosition in currentSpawnConfiguration)
		{
			GameObject instance = Instantiate<GameObject>(currentShape);

			Transform t = instance.transform;
			t.localPosition = spawnPosition.position;
			t.localRotation = spawnPosition.rotation;
			t.SetParent(parent.transform, true);
			t.Rotate(Vector3.up, currentAngle);

			Shape shape = instance.GetComponent<Shape>();
			shape.SetColor(color);
			block.AddShape(shape);
		}

		spawnedBlocks.Add(block);
	}
}
