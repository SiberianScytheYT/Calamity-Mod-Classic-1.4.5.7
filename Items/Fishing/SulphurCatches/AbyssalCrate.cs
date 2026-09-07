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
using Terraria.GameContent.ItemDropRules;
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

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            var postSkeletron = itemLoot.DefineConditionalDropSet(() => NPC.downedBoss3);
            var tier2AcidRain = itemLoot.DefineConditionalDropSet(() => CalamityWorld.downedAquaticScourgeAcidRain);
            
            //Modded materials
            itemLoot.Add(ModContent.ItemType<Items.Placeables.SulphurousSand>(), 1, 5, 10);
            itemLoot.Add(ModContent.ItemType<Items.Placeables.SulphurousSandstone>(), 1, 5, 10);
            itemLoot.Add(ModContent.ItemType<Acidwood>(), 1, 5, 10);
            itemLoot.Add(ItemID.Starfish, 2, 2, 3);
            itemLoot.Add(ItemID.Seashell, 2, 2, 3);
            itemLoot.Add(ItemID.Coral, 2, 2, 3);
            itemLoot.Add(ModContent.ItemType<VictoryShard>(), 2, 2, 3);
			
            itemLoot.AddIf(() => CalamityWorld.downedEoCAcidRain, ModContent.ItemType<SulfuricScale>(), 2, 5, 10);
			
			itemLoot.AddIf(() => CalamityWorld.downedAquaticScourgeAcidRain, ModContent.ItemType<CorrodedFossil>(), 2, 5, 10);
			
            itemLoot.AddIf(() => CalamityWorld.downedCalamitas, ModContent.ItemType<DepthCells>(), 2, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedCalamitas, ModContent.ItemType<Lumenite>(), 2, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedCalamitas, ModContent.ItemType<Items.Placeables.PlantyMush>(), 2, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedCalamitas, ModContent.ItemType<Items.Placeables.Tenebris>(), 2, 5, 10);
            
            itemLoot.AddIf(() => NPC.downedGolemBoss, ModContent.ItemType<CruptixBar>(), 4, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedPolterghast, ModContent.ItemType<ReaperTooth>(), 4, 5, 10);

            // Weapons
            postSkeletron.Add(new OneFromOptionsDropRule(5, 1,
                ModContent.ItemType<Archerfish>(),
                ModContent.ItemType<BallOFugu>(),
                ModContent.ItemType<HerringStaff>(),
                ModContent.ItemType<Lionfish>(),
                ModContent.ItemType<BlackAnurian>()));

            tier2AcidRain.Add(new OneFromOptionsDropRule(5, 1,
                ModContent.ItemType<SkyfinBombers>(),
                ModContent.ItemType<NuclearRod>(),
                ModContent.ItemType<SulphurousGrabber>(),
                ModContent.ItemType<FlakToxicannon>(),
                ModContent.ItemType<SpentFuelContainer>(),
                ModContent.ItemType<SlitheringEels>(),
                ModContent.ItemType<BelchingSaxophone>()));

            // Equipment
            postSkeletron.Add(new OneFromOptionsDropRule(40, 100,
                ModContent.ItemType<StrangeOrb>(),
                ModContent.ItemType<DepthCharm>(),
                ModContent.ItemType<IronBoots>(),
                ModContent.ItemType<AnechoicPlating>()));

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
            itemLoot.Add(ModContent.ItemType<AnechoicCoating>(), 10, 1, 3);
            
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
