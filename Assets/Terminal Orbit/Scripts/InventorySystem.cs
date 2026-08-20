using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Terminal_Orbit.Script
{
    public class InventorySystem : MonoBehaviour
    {
        public TextMeshProUGUI textOutput;

        // Singleton instance for easy access
        public static InventorySystem Instance;

        private List<InventoryItem> _items = new List<InventoryItem>();
        private int _cash;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var exclude = new List<string>()
            {
                "START",
                "END",
                "Bar"
            };
            
            if (exclude.Contains(SceneManager.GetActiveScene().name))
            {
                // do nothing
            }
            else
            {
                textOutput = GameObject.Find("Inventory Text").GetComponent<TextMeshProUGUI>();
                Print();
            }
            
        }

        void OnDestroy()
        {
            // Unsubscribe when the object is destroyed
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Add(InventoryItem item)
        {
            _items.Add(item);
            Print();
        }

        public void AddByName(string productName)
        {
            InventoryItem newItem = new InventoryItem(productName);
            Add(newItem);
        }

        public void AddCash(int amount)
        {
            _cash += amount;
            Print();
        }

        public void RemoveCash(int amount)
        {
            _cash -= amount;
        }

        public bool ItemCheck(string itemName)
        {
            return _items.Any(item => item.itemName == itemName);
        }

        public bool ItemCheck(List<string> itemList)
        {
            foreach (var t in itemList)
            {
                if (_items.Any(item => item.itemName == t) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CashCheck(int amount)
        {
            if (amount < _cash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Print()
        {
            string result = "";

            result += "Cash: " + _cash + Environment.NewLine + Environment.NewLine;

            if (_items.Count == 0)
            {
                result += "Empty Inventory";
            }
            else
            {
                foreach (var t in _items)
                {
                    result += t.itemName + Environment.NewLine;
                }
            }

            textOutput.text = result;
        }
    }
}