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

        public override void RightClick(Player player)
        {
            //Modded materials
			DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.Navystone>(), 10, 30);
			DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.EutrophicSand>(), 10, 30);
            if (CalamityWorld.downedDesertScourge)
            {
				DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<PrismShard>(), 10, 20);
				DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.SeaPrism>(), 5, 10);
            }
            if (Main.hardMode)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<MolluskHusk>(), 0.5f, 5, 15);
            }

            // Weapons
            DropHelper.DropItemFromSetCondition(player.GetSource_FromThis(), player, CalamityWorld.downedCLAMHardMode, 0.2f,
                ModContent.ItemType<ShellfishStaff>(),
                ModContent.ItemType<ClamCrusher>(),
                ModContent.ItemType<Poseidon>(),
                ModContent.ItemType<ClamorRifle>());

            //Bait
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MasterBait, 10, 1, 2);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.JourneymanBait, 5, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<SeaMinnowItem>(), 5, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.ApprenticeBait, 3, 2, 3);

            //Potions
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.ObsidianSkinPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.SwiftnessPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.IronskinPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.NightOwlPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.ShinePotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MiningPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.HeartreachPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.TrapsightPotion, 10, 1, 3); //Dangersense Potion
            if (Main.hardMode)
            {
                DropHelper.DropItem(player.GetSource_FromThis(), player, Main.rand.Next(100) >= 49 ? ItemID.GreaterHealingPotion: ItemID.GreaterManaPotion, 5, 10);
            }
            else
            {
                DropHelper.DropItem(player.GetSource_FromThis(), player, Main.rand.Next(100) >= 49 ? ItemID.HealingPotion : ItemID.ManaPotion, 5, 10);
            }

            //Money
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.SilverCoin, 10, 90);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.GoldCoin, 0.5f, 1, 5);
        }
    }
}
