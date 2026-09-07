using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.HiveMind;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalRD.DropHelper;

namespace CalRD.Items.TreasureBags
{
    public class HiveMindBag : ModItem
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
            itemLoot.Add(ItemID.RottenChunk, 1, 10, 20);
            itemLoot.Add(ItemID.DemoniteBar, 1, 9, 14);
            itemLoot.Add(ModContent.ItemType<TrueShadowScale>(), 1, 30, 40);
            itemLoot.AddIf(() => Main.hardMode, ItemID.CursedFlame, 15, 30);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new WeightedItemStack[]
            {
                ModContent.ItemType<PerfectDark>(),
                ModContent.ItemType<LeechingDagger>(),
                ModContent.ItemType<Shadethrower>(),
                ModContent.ItemType<ShadowdropStaff>(),
                ModContent.ItemType<ShaderainStaff>(),
                ModContent.ItemType<DankStaff>(),
                new WeightedItemStack(ModContent.ItemType<RotBall>(), 1f, 50, 75)
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<RottenBrain>());
            itemLoot.Add(ModContent.ItemType<FilthyGlove>(), 3);

            // Vanity
            itemLoot.Add(ModContent.ItemType<HiveMindMask>(), 7);
        }
    }
}
