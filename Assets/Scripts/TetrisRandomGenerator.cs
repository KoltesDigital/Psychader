using UnityEngine;
using System.Collections.Generic;

public class TetrisRandomGenerator<Type>
{
	private class RouletteItem
	{
		public Type value;
		public float chance;

		public RouletteItem(Type _value, float _chance)
		{
			value = _value;
			chance = _chance;
		}
	}

	private float decrease;
	private List<RouletteItem> items = new List<RouletteItem>();

	public TetrisRandomGenerator(float _decrease)
	{
		decrease = _decrease;
	}

	public void Add(Type value, float chance = 1.0f)
	{
		items.Add(new RouletteItem(value, chance));
	}

	public Type Draw()
	{
		List<RouletteItem> roulette = new List<RouletteItem>();
		float ratio = 1.0f;
		float total = 0.0f;
		
		foreach (RouletteItem item in items)
		{
			total += item.chance * ratio;
			roulette.Add(new RouletteItem(item.value, total));
			ratio *= decrease;
		}

		float r = Random.Range(0.0f, total);
		for (int i = 0, n = roulette.Count - 1; i < n; ++i)
		{
			if (roulette[i].chance > r)
			{
				RouletteItem item = roulette[i];
				items.RemoveAt(i);
				items.Add(item);

				return item.value;
			}
		}

		return roulette[roulette.Count - 1].value;
	}

	public void Shuffle()
	{
		for (int i = items.Count - 1; i > 0; --i)
		{
			int j = Random.Range(0, i + 1);
			RouletteItem value = items[j];
			items[j] = items[i];
			items[i] = value;
		}
	}

}