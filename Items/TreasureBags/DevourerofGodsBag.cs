using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Mounts;
using CalRD.Items.Pets;
using CalRD.Items.Placeables.FurnitureCosmilite;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.DevourerofGods;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class DevourerofGodsBag : ModItem
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
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/TreasureBags/DevourerofGodsBagGlow").Value);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CosmiliteBar>(), 30, 39);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CosmiliteBrick>(), 200, 320);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<Excelsus>(w),
                DropHelper.WeightStack<TheObliterator>(w),
                DropHelper.WeightStack<Deathwind>(w),
                DropHelper.WeightStack<DeathhailStaff>(w),
                DropHelper.WeightStack<StaffoftheMechworm>(w),
                Main.rand.NextBool() ? DropHelper.WeightStack<EradicatorMelee>(w) : DropHelper.WeightStack<Eradicator>(w)
            );

            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Skullmasher>(), DropHelper.RareVariantDropRateInt);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Norfleet>(), DropHelper.RareVariantDropRateInt);
            float dischargeChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<CosmicDischarge>(), CalamityWorld.revenge, dischargeChance);

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<NebulousCore>());
            bool vodka = player.Calamity().fabsolVodka;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Fabsol>(), CalamityWorld.revenge && vodka);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<DevourerofGodsMask>(), 7);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<CosmicPlushie>(), CalamityWorld.death && player.difficulty == 2);
        }
    }
}
