using CalRD.Items.Materials;
using CalRD.Tiles.Crags;
using CalRD.World;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
			//Vanilla materials
            itemLoot.Add(ItemID.Hellstone, 1, 5, 10);
            itemLoot.Add(ItemID.Obsidian, 1, 5, 10);
            itemLoot.Add(ItemID.HellstoneBar, 2, 3, 5);

            //Modded materials
            itemLoot.Add(ModContent.ItemType<DemonicBoneAsh>(), 1, 3, 5);
            itemLoot.AddIf(() => Main.hardMode, ModContent.ItemType<EssenceofChaos>(), 2, 5, 15);
            itemLoot.AddIf(() => Main.hardMode, ModContent.ItemType<BlightedLens>(), 7, 2, 6);
            itemLoot.AddIf(() => CalamityWorld.downedBrimstoneElemental, ModContent.ItemType<UnholyCore>(), 2, 5, 15);
            itemLoot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<Bloodstone>(), 4, 8, 10);

            // Weapons (none)

            //Bait
            itemLoot.Add(ItemID.MasterBait, 10, 1, 2);
            itemLoot.Add(ItemID.JourneymanBait, 5, 1, 3);
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
            itemLoot.Add(ItemID.InfernoPotion, 10, 1, 3);
            
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
