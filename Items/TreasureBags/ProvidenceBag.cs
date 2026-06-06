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

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<UnholyEssence>(), 25, 35);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<DivineGeode>(), 20, 30);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<HolyCollider>(w),
                DropHelper.WeightStack<SolarFlare>(w),
                DropHelper.WeightStack<TelluricGlare>(w),
                DropHelper.WeightStack<BlissfulBombardier>(w),
                DropHelper.WeightStack<PurgeGuzzler>(w),
                DropHelper.WeightStack<DazzlingStabberStaff>(w),
                DropHelper.WeightStack<MoltenAmputator>(w)
            );

            float pristineFuryChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<PristineFury>(), CalamityWorld.revenge, pristineFuryChance);

            // Equipment
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<SamuraiBadge>(), DropHelper.RareVariantDropRateInt);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<BlazingCore>());

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<ProvidenceMask>(), 7);
        }
    }
}
