using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Perforator;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalRD.DropHelper;

namespace CalRD.Items.TreasureBags
{
    public class PerforatorBag : ModItem
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
            itemLoot.Add(ItemID.Vertebrae, 1, 10, 20);
            itemLoot.Add(ItemID.CrimtaneBar, 1, 9, 14);
            itemLoot.Add(ModContent.ItemType<BloodSample>(), 1, 30, 40);
            itemLoot.AddIf(() => Main.hardMode, ItemID.Ichor, 1, 15, 30);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new WeightedItemStack[]
            {
                ModContent.ItemType<VeinBurster>(),
                ModContent.ItemType<BloodyRupture>(),
                ModContent.ItemType<SausageMaker>(),
                ModContent.ItemType<Aorta>(),
                ModContent.ItemType<Eviscerator>(),
                ModContent.ItemType<BloodBath>(),
                ModContent.ItemType<BloodClotStaff>(),
                new WeightedItemStack(ModContent.ItemType<ToothBall>(), 1f, 50, 75)
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<BloodyWormTooth>());
            itemLoot.Add(ModContent.ItemType<BloodstainedGlove>(), 3);

            // Vanity
            itemLoot.Add(ModContent.ItemType<PerforatorMask>(), 7);
            itemLoot.Add(ModContent.ItemType<BloodyVein>(), 10);
        }
    }
}
