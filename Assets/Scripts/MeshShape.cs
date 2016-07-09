using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MeshShape : MonoBehaviour, Shape
{
	public void SetColor(Color color)
	{
		Material material = GetComponent<Renderer>().material;
		material.color = color;
	}

	public void SetRatio(float ratio)
	{
		Vector3 position = transform.localPosition;
		position.y = Mathf.Lerp(1.0f, -1.0f, ratio);
		transform.localPosition = position;
	}
}
