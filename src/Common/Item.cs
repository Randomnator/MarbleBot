﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarbleBot.Common
{
    /// <summary> Represents an inventory item. </summary> 
    public class Item
    {
        /// <summary> The identification number of the item. </summary>
        public uint Id { get; internal set; }
        /// <summary> The name of the item. </summary>
        public string Name { get; }
        /// <summary> The price of the item. </summary>
        public decimal Price { get; }
        /// <summary> The description of the item. </summary>
        public string Description { get; }
        /// <summary> Whether the item is available to buy in the shop or not. </summary>
        public bool OnSale { get; }
        /// <summary> The Stage at which the item can be obtained. </summary>
        public int Stage { get; }
        /// <summary> The location the item can be found in during a Scavenge game. </summary>
        public ScavengeLocation ScavengeLocation { get; }
        /// <summary> The quantity produced of this item upon being crafted. </summary>
        public uint CraftingProduced { get; }
        /// <summary> The crafting recipe - a key value pair of item IDs and their necessary quantities. </summary>
        public Dictionary<string, int> CraftingRecipe { get; }
        /// <summary> Which crafting station is required to craft the item. </summary>
        public int CraftingStationRequired { get; }

        [JsonConstructor]
        public Item(uint id = 0, string name = "", decimal price = 0m, string description = "", bool onSale = false,
            int stage = 1, ScavengeLocation scavengeLocation = ScavengeLocation.None, uint craftingProduced = 0u,
            Dictionary<string, int> craftingRecipe = null, int craftingStationRequired = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            OnSale = onSale;
            Stage = stage;
            ScavengeLocation = scavengeLocation;
            CraftingProduced = craftingProduced;
            CraftingRecipe = craftingRecipe ?? new Dictionary<string, int>();
            CraftingStationRequired = craftingStationRequired;
        }

        public Item(Item baseItem, uint id = 0, bool onSale = false)
        {
            Id = id == 0 ? baseItem.Id : id;
            Name = baseItem.Name;
            Price = baseItem.Price;
            Description = baseItem.Description;
            OnSale = onSale == baseItem.OnSale ? onSale : baseItem.OnSale;
            Stage = baseItem.Stage;
            ScavengeLocation = baseItem.ScavengeLocation;
            CraftingProduced = baseItem.CraftingProduced;
            CraftingRecipe = baseItem.CraftingRecipe;
            CraftingStationRequired = baseItem.CraftingStationRequired;
        }

        /// <summary> Converts this item into a string representation. </summary>
        public override string ToString()
            => $"`[{Id.ToString("000")}]` **{Name}**";
    }
}