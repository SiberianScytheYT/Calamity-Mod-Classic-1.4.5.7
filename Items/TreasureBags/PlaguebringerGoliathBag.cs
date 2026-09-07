using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.PlaguebringerGoliath;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class PlaguebringerGoliathBag : ModItem
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
            itemLoot.Add(ModContent.ItemType<PlagueCellCluster>(), 1, 13, 17);
            itemLoot.Add(ModContent.ItemType<InfectedArmorPlating>(), 1, 16, 20);
            itemLoot.Add(ItemID.Stinger, 1, 4, 8);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<VirulentKatana>(), // Virulence
                ModContent.ItemType<DiseasedPike>(),
                ModContent.ItemType<ThePlaguebringer>(), // Pandemic
                ModContent.ItemType<Malevolence>(),
                ModContent.ItemType<PestilentDefiler>(),
                ModContent.ItemType<TheHive>(),
                ModContent.ItemType<MepheticSprayer>(), // Blight Spewer
                ModContent.ItemType<PlagueStaff>(),
                ModContent.ItemType<FuelCellBundle>(),
                ModContent.ItemType<InfectedRemote>(),
                ModContent.ItemType<TheSyringe>()
            }));

            int malachiteChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<Malachite>(),  malachiteChance);

            // Equipment
            itemLoot.Add(ModContent.ItemType<ToxicHeart>());
            itemLoot.Add(ModContent.ItemType<BloomStone>(), 10);

            // Vanity
            itemLoot.Add(ModContent.ItemType<PlaguebringerGoliathMask>(), 7);
            itemLoot.Add(ModContent.ItemType<PlagueCaller>(), 10);
        }
    }
}
