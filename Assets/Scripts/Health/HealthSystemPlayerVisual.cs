using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystemPlayerVisual : MonoBehaviour
{
    public HealthSystem healthSystem;

    public GameObject heartPrefab;
    public Transform heartContainer;

    public Sprite FullHeart;
    public Sprite HalfHeart;
    public Sprite EmptyHeart;

    private List<Image> heartImages = new List<Image>();

    private void Start()
    {
        SpawnHearts();
        UpdateHealthVisual(healthSystem.maxHealth);
        healthSystem.onHealthValueUpdated += UpdateHealthVisual;
    }

    public void SpawnHearts()
    {
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();
        int heartCount = Mathf.CeilToInt(healthSystem.maxHealth / 2f);

        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heart.GetComponent<Image>();
            heartImages.Add(heartImage);
        }
    }

    public void UpdateHealthVisual(int health)
    {

        for (int i = 0; i < heartImages.Count; i++)
        {
            int heartValue = (i + 1) * 2;

            if (health >= heartValue)
            {
                heartImages[i].sprite = FullHeart;
            }
            else if (health == heartValue - 1)
            {
                heartImages[i].sprite = HalfHeart;
            }
            else
            {
                heartImages[i].sprite = EmptyHeart;
            }
        }
    }
}
