using CalRD.Items.Materials;
using CalRD.Items.Critters;
using CalRD.Items.Placeables;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.Tiles.SunkenSea;
using CalRD.World;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.SunkenSeaCatches
{
	public class SunkenCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sunken Crate");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = 2;
            Item.value = Item.sellPrice(gold: 1);
            Item.createTile = ModContent.TileType<SunkenCrateTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            var postHardmodeClam = itemLoot.DefineConditionalDropSet(() => CalamityWorld.downedCLAMHardMode);
            
            //Modded materials
            itemLoot.Add(ModContent.ItemType<Items.Placeables.Navystone>(), 1, 10, 30);
            itemLoot.Add(ModContent.ItemType<Items.Placeables.EutrophicSand>(), 1, 10, 30);
            
            itemLoot.AddIf(() => CalamityWorld.downedDesertScourge, ModContent.ItemType<PrismShard>(), 1, 10, 20);
            itemLoot.AddIf(() => CalamityWorld.downedDesertScourge, ModContent.ItemType<Items.Placeables.SeaPrism>(), 1, 5, 10);
            itemLoot.AddIf(() => Main.hardMode, ModContent.ItemType<MolluskHusk>(), 2, 5, 15);

            // Weapons
            postHardmodeClam.Add(new OneFromOptionsNotScaledWithLuckDropRule(5,
                ModContent.ItemType<ShellfishStaff>(),
                ModContent.ItemType<ClamCrusher>(),
                ModContent.ItemType<Poseidon>(),
                ModContent.ItemType<ClamorRifle>()));

            //Bait
            itemLoot.Add(ItemID.MasterBait, 10, 1, 2);
            itemLoot.Add(ItemID.JourneymanBait, 5, 1, 3);
            itemLoot.Add(ModContent.ItemType<SeaMinnowItem>(), 5, 1, 3);
            itemLoot.Add(ItemID.ApprenticeBait, 3, 2, 3);

            //Potions
            itemLoot.Add(ItemID.ObsidianSkinPotion, 10, 1, 3);
            itemLoot.Add(ItemID.SwiftnessPotion, 10, 1, 3);
            itemLoot.Add(ItemID.IronskinPotion, 10, 1, 3);
            itemLoot.Add(ItemID.NightOwlPotion, 10, 1, 3);
            itemLoot.Add(ItemID.ShinePotion, 10, 1, 3);
            itemLoot.Add(ItemID.MiningPotion, 10, 1, 3);
            itemLoot.Add(ItemID.HeartreachPotion, 10, 1, 3);
            itemLoot.Add(ItemID.TrapsightPotion, 10, 1, 3); //Dangersense Potion
            
            itemLoot.AddIf(() => Main.hardMode, ItemID.GreaterHealingPotion, 1, 5, 10);
            itemLoot.AddIf(() => Main.hardMode, ItemID.GreaterManaPotion, 1, 5, 10);
            itemLoot.AddIf(() => !Main.hardMode, ItemID.HealingPotion, 1, 5, 10);
            itemLoot.AddIf(() => !Main.hardMode, ItemID.ManaPotion, 1, 5, 10);

            //Money
            itemLoot.Add(ItemID.SilverCoin, 1, 10, 90);
            itemLoot.Add(ItemID.GoldCoin, 2, 1, 5);
        }
    }
}
