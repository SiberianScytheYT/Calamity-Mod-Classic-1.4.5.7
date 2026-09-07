
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Cryogen;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class CryogenBag : ModItem
    { 
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 9;
            Item.expert = true;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<CryoBar>(), 1, 20, 40);
            itemLoot.Add(ModContent.ItemType<EssenceofEleum>(), 1, 5, 9);
            itemLoot.Add(ItemID.FrostCore);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<Avalanche>(),
                ModContent.ItemType<GlacialCrusher>(),
                ModContent.ItemType<EffluviumBow>(),
                ModContent.ItemType<BittercoldStaff>(),
                ModContent.ItemType<SnowstormStaff>(),
                ModContent.ItemType<Icebreaker>()
            }));

            int divinityChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<ColdDivinity>(), divinityChance);

            // Equipment
            itemLoot.Add(ModContent.ItemType<SoulofCryogen>());
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<FrostFlare>());
            itemLoot.Add(ModContent.ItemType<CryoStone>(), 10);
            itemLoot.Add(ModContent.ItemType<Regenator>(), DropHelper.RareVariantDropRateInt);

            // Vanity
            itemLoot.Add(ModContent.ItemType<CryogenMask>(), 7);

            // Other
            itemLoot.Add(ItemID.FrozenKey, 5);
        }
    }
}
