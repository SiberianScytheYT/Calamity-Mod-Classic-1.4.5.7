using CalRD.Dusts;
using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Brimlash : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimlash");
/*
            Tooltip.SetDefault("Fires a brimstone bolt that explodes into more bolts on death");
*/
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 54;
            Item.damage = 64;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.shoot = ModContent.ProjectileType<BrimlashProj>();
            Item.shootSpeed = 15f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UnholyCore>(), 4);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, (int)CalamityDusts.Brimstone);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
        }
    }
}
