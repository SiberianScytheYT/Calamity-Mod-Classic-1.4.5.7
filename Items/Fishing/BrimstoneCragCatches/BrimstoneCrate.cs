using CalRD.Items.Materials;
using CalRD.Tiles.Crags;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.BrimstoneCragCatches
{
    public class BrimstoneCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Crate");
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
            Item.createTile = ModContent.TileType<BrimstoneCrateTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
			//Vanilla materials
			DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Hellstone, 5, 10);
			DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Obsidian, 5, 10);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.HellstoneBar, 2, 3, 5);

            //Modded materials
			DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<DemonicBoneAsh>(), 3, 5);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<EssenceofChaos>(), Main.hardMode, 0.5f, 5, 15);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<BlightedLens>(), Main.hardMode, 0.15f, 2, 6);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<UnholyCore>(), CalamityWorld.downedBrimstoneElemental, 0.5f, 5, 15);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 0.25f, 8, 10);

            // Weapons (none)

            //Bait
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MasterBait, 10, 1, 2);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.JourneymanBait, 5, 1, 3);
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
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.InfernoPotion, 10, 1, 3);
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
