using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Mounts;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.BrimstoneElemental;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class BrimstoneWaifuBag : ModItem
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
            Item.expert = true;
            Item.rare = 9;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<EssenceofChaos>(), 1, 5, 9);
            itemLoot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<Bloodstone>(), 1, 25, 35);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<Brimlance>(),
                ModContent.ItemType<SeethingDischarge>(),
                ModContent.ItemType<DormantBrimseeker>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<Abaddon>());
            itemLoot.Add(ModContent.ItemType<Gehenna>());
            itemLoot.Add(ModContent.ItemType<RoseStone>(), 10);
            itemLoot.AddIf(() => CalamityWorld.revenge && CalamityWorld.downedProvidence, ModContent.ItemType<Brimrose>());

            // Vanity
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<CharredRelic>());
            itemLoot.Add(ModContent.ItemType<BrimstoneWaifuMask>(), 7);
        }
    }
}
