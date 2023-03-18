using UnityEngine;

namespace Game.Scripts.Data.StaticData
{
    public abstract class ShopItemData : ScriptableObject
    {
        [Header("Base Shop Settings")]
        //public ShopDataType DataType;
        public string Name;
        public Sprite Icon;
        public PriceAmount Price;

        [Space(2)] 
        [TextArea] 
        public string Description;
    }

    public enum ShopDataType
    {
        Product,
        Upgrade
    }
    
    [System.Serializable]
    public class PriceAmount
    {
        public int SeedPrice;
        public int CoinPrice;
    }
}