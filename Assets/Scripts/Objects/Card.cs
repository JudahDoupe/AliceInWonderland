using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{

    public static List<Card> Cards = new List<Card>();
    public static bool Complete;
    public int Number;
    public bool Active = true;

	void OnClick ()
	{
	    var min = Number;
	    foreach (var card in Cards)
	    {
	        if (card.Active && card.Number < min)
	            min = card.Number;
	    }

	    if (min == Number)
	    {
	        Active = false;
	        gameObject.SetActive(false);
	    }
	    else
	    {
	        foreach (var card in Cards)
	        {
	            card.gameObject.SetActive(true);
	            card.Active = true;
	        }
	    }
	    if (Cards.All(x => !x.Active))
	        Complete = true;
	}

    public static void Reset()
    {
        foreach (var card in Cards)
            Destroy(card);
        Complete = false;
        Cards = new List<Card>();
    }
}
