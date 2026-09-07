using CalRD.World;
using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.AquaticScourge;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Armor.Vanity;
using Terraria.GameContent.ItemDropRules;

namespace CalRD.Items.TreasureBags
{
    public class AquaticScourgeBag : ModItem
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
            // Materials
            itemLoot.Add(ModContent.ItemType<VictoryShard>(), 1, 15, 25);
            itemLoot.Add(ItemID.Coral, 1, 7, 11);
            itemLoot.Add(ItemID.Seashell, 1, 7, 11);
            itemLoot.Add(ItemID.Starfish, 1, 7, 11);

            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<SubmarineShocker>(),
                ModContent.ItemType<Barinautical>(),
                ModContent.ItemType<Downpour>(),
                ModContent.ItemType<DeepseaStaff>(),
                ModContent.ItemType<ScourgeoftheSeas>()
            }));

            int searingChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<SeasSearing>(),  searingChance);

            // Equipment
            itemLoot.Add(ModContent.ItemType<AquaticEmblem>());
            itemLoot.Add(ModContent.ItemType<AeroStone>(), 8);
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<CorrosiveSpine>(), 4);

            // Vanity
            itemLoot.Add(ModContent.ItemType<AquaticScourgeMask>(), 7);

            // Fishing
            itemLoot.Add(ModContent.ItemType<BleachedAnglingKit>());
        }
    }
}
