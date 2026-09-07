using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Providence;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class ProvidenceBag : ModItem
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
            itemLoot.Add(ModContent.ItemType<UnholyEssence>(), 1, 25, 35);
            itemLoot.Add(ModContent.ItemType<DivineGeode>(), 1, 20, 30);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<HolyCollider>(),
                ModContent.ItemType<SolarFlare>(),
                ModContent.ItemType<TelluricGlare>(),
                ModContent.ItemType<BlissfulBombardier>(),
                ModContent.ItemType<PurgeGuzzler>(),
                ModContent.ItemType<DazzlingStabberStaff>(),
                ModContent.ItemType<MoltenAmputator>()
            }));

            int pristineFuryChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<PristineFury>(), pristineFuryChance);

            // Equipment
            itemLoot.Add(ModContent.ItemType<SamuraiBadge>(), DropHelper.RareVariantDropRateInt);
            itemLoot.Add(ModContent.ItemType<BlazingCore>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<ProvidenceMask>(), 7);
        }
    }
}
