using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Critters;
using CalRD.Items.Placeables;
using CalRD.Items.Placeables.Ores;
using CalRD.Items.Potions;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Tiles.Astral;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class AstralCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Astral Crate");
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
            Item.createTile = ModContent.TileType<AstralCrateTile>();
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Stardust>(), 10, 20);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.FallenStar, 10, 20);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Meteorite, 0.5f, 10, 20);
			DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MeteoriteBar, 0.25f, 5, 10);
            if (CalamityWorld.downedAstrageldon)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AstralJelly>(), 0.5f, 5, 10);
            }
            if (CalamityWorld.downedStarGod)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AstralOre>(), 0.5f, 10, 20);
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AstralBar>(), 0.25f, 5, 10);
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<MeldBlob>(), 0.25f, 5, 10);
            }

            // Weapons
            DropHelper.DropItemFromSetCondition(player.GetSource_FromThis(), player, CalamityWorld.downedAstrageldon, 0.2f,
                ModContent.ItemType<StellarKnife>(),
                ModContent.ItemType<AstralachneaStaff>(),
                ModContent.ItemType<TitanArm>(),
                ModContent.ItemType<HivePod>(),
                ModContent.ItemType<AstralScythe>(),
                ModContent.ItemType<StellarCannon>(),
                ModContent.ItemType<StarbusterCore>());

            //Pet
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AstrophageItem>(), 10);

            //Bait
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<TwinklerItem>(), 5, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.EnchantedNightcrawler, 5, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<ArcturusAstroidean>(), 5, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Firefly, 3, 1, 3);

            //Potions
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.ObsidianSkinPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.SwiftnessPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.IronskinPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.NightOwlPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.ShinePotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.MiningPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.HeartreachPotion, 10, 1, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.TrapsightPotion, 10, 1, 3); //Dangersense Potion
            if (CalamityWorld.downedStarGod)
            {
                DropHelper.DropItem(player.GetSource_FromThis(), player, Main.rand.Next(100) >= 49 ? ItemID.SuperHealingPotion : ItemID.SuperManaPotion, 5, 10);
            }
            else
            {
                DropHelper.DropItem(player.GetSource_FromThis(), player, Main.rand.Next(100) >= 49 ? ItemID.GreaterHealingPotion : ItemID.GreaterManaPotion, 5, 10);
            }
            if (CalamityWorld.downedAstrageldon)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AstralInjection>(), 4, 1, 3);
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<GravityNormalizerPotion>(), 4, 1, 3);
            }

            //Money
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.SilverCoin, 10, 90);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.GoldCoin, 0.5f, 1, 5);
        }
    }
}
