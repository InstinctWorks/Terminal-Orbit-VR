using System;

namespace Terminal_Orbit.Script
{
    [Serializable]
    public class InventoryItem
    {
        public string itemName;
        public int itemID; // Or any other relevant data

        public InventoryItem(string name)
        {
            itemName = name;
        }
    
        public InventoryItem(string name, int id)
        {
            itemName = name;
            itemID = id;
        }
    }
}