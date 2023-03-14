using Game.Scripts.Logic.Production;
using UnityEngine;

namespace Game.Scripts.Data.StaticData.Product
{
    [CreateAssetMenu(fileName = "Product", menuName = "Products/New Product")]
    public class ProductItemData : ShopItemData
    {
        [Space(4)]
        [Header("Base Product Settings")]
        public ProductType ProductType;
        public Material ViewMaterial;

        [Space(4)] 
        [Header("Production Settings")]
        public int GrowTime;
        public DropData DropData;
    }

    [System.Serializable]
    public class DropData
    {
        public int SeedDropChance;
        public int Coin;
    }
}