using CalRD.Items.Materials;
using CalRD.Items.Accessories;
using CalRD.Items.Pets;
using CalRD.Items.Potions;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.Tiles.Abyss;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.SulphurCatches
{
	public class AbyssalCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Crate");
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
            Item.createTile = ModContent.TileType<AbyssalCrateTile>();
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.SulphurousSand>(), 5, 10);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.SulphurousSandstone>(), 5, 10);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Acidwood>(), 5, 10);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Starfish, 0.5f, 2, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Seashell, 0.5f, 2, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.Coral, 0.5f, 2, 3);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<VictoryShard>(), 0.5f, 2, 3);
			if (CalamityWorld.downedEoCAcidRain)
			{
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<SulfuricScale>(), 0.5f, 5, 10);
			}
			if (CalamityWorld.downedAquaticScourgeAcidRain)
			{
				DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<CorrodedFossil>(), 0.5f, 5, 10);
			}
            if (CalamityWorld.downedCalamitas)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<DepthCells>(), 0.5f, 5, 10);
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Lumenite>(), 0.5f, 5, 10);
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.PlantyMush>(), 0.5f, 5, 10);
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Items.Placeables.Tenebris>(), 0.5f, 5, 10);
            }
            if (NPC.downedGolemBoss)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<CruptixBar>(), 0.25f, 5, 10);
            }
            if (CalamityWorld.downedPolterghast)
            {
                DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<ReaperTooth>(), 0.25f, 5, 10);
            }

            // Weapons
            DropHelper.DropItemFromSetCondition(player.GetSource_FromThis(), player, NPC.downedBoss3, 0.2f,
                ModContent.ItemType<Archerfish>(),
                ModContent.ItemType<BallOFugu>(),
                ModContent.ItemType<HerringStaff>(),
                ModContent.ItemType<Lionfish>(),
                ModContent.ItemType<BlackAnurian>());

            DropHelper.DropItemFromSetCondition(player.GetSource_FromThis(), player, CalamityWorld.downedAquaticScourgeAcidRain, 0.2f,
                ModContent.ItemType<SkyfinBombers>(),
                ModContent.ItemType<NuclearRod>(),
                ModContent.ItemType<SulphurousGrabber>(),
                ModContent.ItemType<FlakToxicannon>(),
                ModContent.ItemType<SpentFuelContainer>(),
                ModContent.ItemType<SlitheringEels>(),
                ModContent.ItemType<BelchingSaxophone>());

            // Equipment
            DropHelper.DropItemFromSetCondition(player.GetSource_FromThis(), player, NPC.downedBoss3, 0.4f,
                ModContent.ItemType<StrangeOrb>(),
                ModContent.ItemType<DepthCharm>(),
                ModContent.ItemType<IronBoots>(),
                ModContent.ItemType<AnechoicPlating>());

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
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AnechoicCoating>(), 10, 1, 3);
            if (Main.hardMode)
            {
                DropHelper.DropItem(player.GetSource_FromThis(), player, Main.rand.Next(100) >= 49 ? ItemID.GreaterHealingPotion : ItemID.GreaterManaPotion, 5, 10);
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
