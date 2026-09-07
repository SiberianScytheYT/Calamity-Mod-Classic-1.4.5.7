
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.Crabulon;
using CalRD.World;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class CrabulonBag : ModItem
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
            itemLoot.Add(ItemID.GlowingMushroom, 1, 25, 35);
            itemLoot.Add(ItemID.MushroomGrassSeeds, 1, 5, 10);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<MycelialClaws>(),
                ModContent.ItemType<Fungicide>(),
                ModContent.ItemType<HyphaeRod>(),
                ModContent.ItemType<Mycoroot>(),
                ModContent.ItemType<Shroomerang>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<FungalClump>());
            
            itemLoot.AddIf(info => CalamityWorld.revenge && !info.player.Calamity().rageBoostOne, ModContent.ItemType<MushroomPlasmaRoot>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<CrabulonMask>(), 7);
        }
    }
}
