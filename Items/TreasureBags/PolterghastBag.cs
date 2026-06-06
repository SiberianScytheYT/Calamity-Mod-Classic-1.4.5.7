using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Polterghast;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class PolterghastBag : ModItem
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

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/TreasureBags/PolterghastBagGlow").Value);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<RuinousSoul>(), 10, 20);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Phantoplasm>(), 20, 30);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<TerrorBlade>(w),
                DropHelper.WeightStack<BansheeHook>(w),
                DropHelper.WeightStack<DaemonsFlame>(w),
                DropHelper.WeightStack<FatesReveal>(w),
                DropHelper.WeightStack<GhastlyVisage>(w),
                DropHelper.WeightStack<EtherealSubjugator>(w),
                DropHelper.WeightStack<GhoulishGouger>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Affliction>());
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Ectoheart>(), CalamityWorld.revenge && !player.Calamity().adrenalineBoostThree);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<PolterghastMask>(), 7);
        }
    }
}
