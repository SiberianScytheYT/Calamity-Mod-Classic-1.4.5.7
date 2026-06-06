using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class FeralthornClaymore : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Feralthorn Claymore");
/*
            Tooltip.SetDefault("Summons thorns on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.damage = 63;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.useTurn = true;
            Item.knockBack = 7.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 44);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 300);
            for (int x = 0; x < 2; x++)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.position.X + 40f + (float)Main.rand.Next(0, 151), player.position.Y + 36f, 0f, -18f, ModContent.ProjectileType<ThornBase>(), (int)(Item.damage * player.MeleeDamage() * 0.2), 0f, Main.myPlayer, 0f, 0f);
            }
            for (int x = 0; x < 2; x++)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.position.X - 40f + (float)Main.rand.Next(-150, 1), player.position.Y + 36f, 0f, -18f, ModContent.ProjectileType<ThornBase>(), (int)(Item.damage * player.MeleeDamage() * 0.2), 0f, Main.myPlayer, 0f, 0f);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Venom, 300);
            for (int x = 0; x < 2; x++)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.position.X + 40f + (float)Main.rand.Next(0, 151), player.position.Y + 36f, 0f, -18f, ModContent.ProjectileType<ThornBase>(), (int)(Item.damage * player.MeleeDamage() * 0.2), 0f, Main.myPlayer, 0f, 0f);
            }
            for (int x = 0; x < 2; x++)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.position.X - 40f + (float)Main.rand.Next(-150, 1), player.position.Y + 36f, 0f, -18f, ModContent.ProjectileType<ThornBase>(), (int)(Item.damage * player.MeleeDamage() * 0.2), 0f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
