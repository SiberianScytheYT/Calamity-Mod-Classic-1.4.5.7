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
using Terraria.GameContent.ItemDropRules;
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

        public override bool CanRightClick() => true;

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<CosmiliteBar>(), 1, 30, 39);
            itemLoot.Add(ModContent.ItemType<CosmiliteBrick>(), 1, 200, 320);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<Excelsus>(),
                ModContent.ItemType<TheObliterator>(),
                ModContent.ItemType<Deathwind>(),
                ModContent.ItemType<DeathhailStaff>(),
                ModContent.ItemType<StaffoftheMechworm>(),
                ModContent.ItemType<EradicatorMelee>(),
                ModContent.ItemType<Eradicator>()
            }));

            itemLoot.Add(ModContent.ItemType<Skullmasher>(), DropHelper.RareVariantDropRateInt);
            itemLoot.Add(ModContent.ItemType<Norfleet>(), DropHelper.RareVariantDropRateInt);
            int dischargeChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<CosmicDischarge>(),  dischargeChance);

            // Equipment
            itemLoot.Add(ModContent.ItemType<NebulousCore>());
            itemLoot.AddIf(info => CalamityWorld.revenge && info.player.Calamity().fabsolVodka, ModContent.ItemType<Fabsol>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<DevourerofGodsMask>(), 7);
            itemLoot.AddIf(info => CalamityWorld.death && info.player.difficulty == 2, ModContent.ItemType<CosmicPlushie>());
        }
    }
}
