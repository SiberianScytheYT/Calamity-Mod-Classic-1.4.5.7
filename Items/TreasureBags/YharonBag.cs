using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Yharon;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class YharonBag : ModItem
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

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<DragonRage>(w),
                DropHelper.WeightStack<TheBurningSky>(w),
                DropHelper.WeightStack<DragonsBreath>(w),
                DropHelper.WeightStack<ChickenCannon>(w),
                DropHelper.WeightStack<PhoenixFlameBarrage>(w),
                DropHelper.WeightStack<AngryChickenStaff>(w), // Yharon Kindle Staff
                DropHelper.WeightStack<ProfanedTrident>(w), // Infernal Spear
                DropHelper.WeightStack<FinalDawn>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<YharimsGift>());

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<YharonMask>(), 7);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<ForgottenDragonEgg>(), 10);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<FoxDrive>(), CalamityWorld.revenge);
        }
    }
}
