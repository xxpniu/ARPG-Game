using System.Collections.Generic;

public  sealed class GRandomer
{
    private static System.Random _randomer = new System.Random();

    public static int RandomMinAndMax(int min, int max)
    {
        return _randomer.Next(min, max);
    }

    public static T RandomList<T>(List<T> list)
	{ 
		return list [_randomer.Next (list.Count)];
	}

    public static T RandomArray<T>(T[] array)
	{
		return array [_randomer.Next (array.Length)];
	}

    public static bool Probability10(int value)
	{
		if (value <= 0)
			return false;
		if (value >= 10)
			return true;
		return _randomer.Next (0, 10) <= value;
	}

    public static bool Probability100(int value)
	{
		if (value <= 0)
			return false;
		if (value >= 100)
			return true;
		return _randomer.Next (0, 100) <= value;
	}

    public static bool Probability10000(int value)
	{
		if (value <= 0)
			return false;
		if (value >= 10000)
			return true;
		return _randomer.Next (0, 10000) <= value;
	}

    public static int RandPro(int[] pro)
	{
		int count = 0;
		foreach (var i in pro) {
			if (i >= 0)
				count += i;
			
		}

		var result = RandomMinAndMax (0, count);
		int last = 0, cur = 0;
		for (var i = 0; i < pro.Length; i++) {
			last = cur;
			cur += pro [i];
			if (last >= result && cur > result)
				return i;
		}

		//Debug.LogError ("Input error");
		return 0;
            
	}
}

