using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Calamitas;
using CalRD.NPCs.NormalNPCs;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class CalamitasBag : ModItem
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
            itemLoot.Add(ModContent.ItemType<CalamityDust>(), 1, 14, 18);
            itemLoot.Add(ModContent.ItemType<BlightedLens>(), 1, 1, 3);
            itemLoot.Add(ModContent.ItemType<EssenceofChaos>(), 1, 5, 9);
            itemLoot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<Bloodstone>(), 1, 35, 45);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<TheEyeofCalamitas>(),
                ModContent.ItemType<Animosity>(),
                ModContent.ItemType<CalamitasInferno>(),
                ModContent.ItemType<BlightedEyeStaff>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<CalamityRing>());
            itemLoot.Add(ModContent.ItemType<ChaosStone>(), 10);

            // Vanity
            itemLoot.Add(ModContent.ItemType<CalamitasMask>(), 7);
        }
    }
}
