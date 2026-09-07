using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.SlimeGod;
using CalRD.World;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class SlimeGodBag : ModItem
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
            itemLoot.Add(ItemID.Gel, 1, 30, 60);
            itemLoot.Add(ModContent.ItemType<PurifiedGel>(), 1, 35, 55);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<OverloadedBlaster>(),
                ModContent.ItemType<AbyssalTome>(),
                ModContent.ItemType<EldritchTome>(),
                ModContent.ItemType<CorroslimeStaff>(),
                ModContent.ItemType<CrimslimeStaff>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<ManaOverloader>());
            itemLoot.AddIf(info => CalamityWorld.revenge && !info.player.Calamity().adrenalineBoostOne, ModContent.ItemType<ElectrolyteGelPack>());

            // Vanity
            itemLoot.Add(ItemDropRule.OneFromOptions(7, ModContent.ItemType<SlimeGodMask>(), ModContent.ItemType<SlimeGodMask2>()));

            // Other
        }
    }
}
