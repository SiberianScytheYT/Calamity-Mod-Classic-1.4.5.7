using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Leviathan;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class LeviathanBag : ModItem
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

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<Greentide>(),
                ModContent.ItemType<Leviatitan>(),
                ModContent.ItemType<SirensSong>(),
                ModContent.ItemType<Atlantis>(),
                ModContent.ItemType<GastricBelcherStaff>(),
                ModContent.ItemType<BrackishFlask>(),
                ModContent.ItemType<LeviathanTeeth>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<LeviathanAmbergris>());
            itemLoot.Add(ModContent.ItemType<LureofEnthrallment>(), 3);
            int communityChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<TheCommunity>(), communityChance);

            // Vanity
            itemLoot.Add(ModContent.ItemType<LeviathanMask>(), 7);
            itemLoot.Add(ModContent.ItemType<AnahitaMask>(), 7);

            // Fishing
            itemLoot.Add(ItemID.HotlineFishingHook, 10);
            itemLoot.Add(ItemID.BottomlessBucket, 10);
            itemLoot.Add(ItemID.SuperAbsorbantSponge, 10);
            itemLoot.Add(ItemID.FishingPotion, 5, 5, 8);
            itemLoot.Add(ItemID.SonarPotion, 5, 5, 8);
            itemLoot.Add(ItemID.CratePotion, 5, 5, 8);
        }
    }
}
