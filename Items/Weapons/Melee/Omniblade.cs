using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Omniblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Omniblade");
/*
            Tooltip.SetDefault("An ancient blade forged by the legendary Omnir");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.damage = 63;
            Item.crit += 45;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 146;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            hitbox = CalamityUtils.FixSwingHitbox(102, 102);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 360);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<WhisperingDeath>(), 360);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Katana);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 20);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
