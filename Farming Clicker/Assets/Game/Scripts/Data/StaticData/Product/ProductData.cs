using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Data.StaticData.Product
{
    [CreateAssetMenu(fileName = "Product", menuName = "Products/New Product")]
    public class ProductData : ScriptableObject
    {
        [Header("Base Settings")]
        public ProductType ProductType;
        public string Name;
        public Material ViewMaterial;
        public Sprite Icon;

        [Space(4)] 
        [Header("Production Settings")]
        public int GrowTime;
        public PriceAmount Price;
        public DropData DropData;
    }

    [System.Serializable]
    public class PriceAmount
    {
        public int SeedPrice;
        public int CoinPrice;
    }

    [System.Serializable]
    public class DropData
    {
        public int SeedDropChance;
        public int Coin;
    }
}