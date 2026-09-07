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
using Terraria.GameContent.ItemDropRules;
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

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            Player player;
            var postAstrumAureus = itemLoot.DefineConditionalDropSet(() => CalamityWorld.downedAstrageldon);
            
            //Modded materials
            itemLoot.Add(ModContent.ItemType<Stardust>(), 1, 10, 20);
            itemLoot.Add(ItemID.FallenStar, 1, 10, 20);
			itemLoot.Add(ItemID.Meteorite, 2, 10, 20);
            itemLoot.Add(ItemID.MeteoriteBar, 4, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedAstrageldon, ModContent.ItemType<AstralJelly>(), 2, 5, 10);
            
            itemLoot.AddIf(() => CalamityWorld.downedStarGod, ModContent.ItemType<AstralOre>(), 2, 10, 20);
            itemLoot.AddIf(() => CalamityWorld.downedStarGod, ModContent.ItemType<AstralBar>(), 4, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedStarGod, ModContent.ItemType<MeldBlob>(), 4, 5, 10);

            // Weapons
            postAstrumAureus.Add(new OneFromOptionsDropRule(5, 1,
                ModContent.ItemType<StellarKnife>(),
                ModContent.ItemType<AstralachneaStaff>(),
                ModContent.ItemType<TitanArm>(),
                ModContent.ItemType<HivePod>(),
                ModContent.ItemType<AstralScythe>(),
                ModContent.ItemType<StellarCannon>(),
                ModContent.ItemType<StarbusterCore>()));

            //Pet
            itemLoot.Add(ModContent.ItemType<AstrophageItem>(), 10);

            //Bait
            itemLoot.Add(ModContent.ItemType<TwinklerItem>(), 5, 1, 3);
            itemLoot.Add(ItemID.EnchantedNightcrawler, 5, 1, 3);
            itemLoot.Add(ModContent.ItemType<ArcturusAstroidean>(), 5, 1, 3);
            itemLoot.Add(ItemID.Firefly, 3, 1, 3);

            //Potions
            itemLoot.Add(ItemID.ObsidianSkinPotion, 10, 1, 3);
            itemLoot.Add(ItemID.SwiftnessPotion, 10, 1, 3);
            itemLoot.Add(ItemID.IronskinPotion, 10, 1, 3);
            itemLoot.Add(ItemID.NightOwlPotion, 10, 1, 3);
            itemLoot.Add(ItemID.ShinePotion, 10, 1, 3);
            itemLoot.Add(ItemID.MiningPotion, 10, 1, 3);
            itemLoot.Add(ItemID.HeartreachPotion, 10, 1, 3);
            itemLoot.Add(ItemID.TrapsightPotion, 10, 1, 3); //Dangersense Potion
            
            itemLoot.AddIf(() => CalamityWorld.downedStarGod, ItemID.SuperHealingPotion, 1, 5, 10);
            itemLoot.AddIf(() => CalamityWorld.downedStarGod, ItemID.SuperManaPotion, 1, 5, 10);
            itemLoot.AddIf(() => !CalamityWorld.downedStarGod, ItemID.GreaterHealingPotion, 1, 5, 10);
            itemLoot.AddIf(() => !CalamityWorld.downedStarGod, ItemID.GreaterManaPotion, 1, 5, 10);
            
            itemLoot.AddIf(() => CalamityWorld.downedAstrageldon, ModContent.ItemType<AstralInjection>(), 4, 1, 3);
            itemLoot.AddIf(() => CalamityWorld.downedAstrageldon, ModContent.ItemType<GravityNormalizerPotion>(), 4, 1, 3);

            //Money
            itemLoot.Add(ItemID.SilverCoin, 1, 10, 90);
            itemLoot.Add(ItemID.GoldCoin, 2, 1, 5);
        }
    }
}
