using CalRD.Items.Materials;
using CalRD.Projectiles.Healing;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class PhoenixBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phoenix Blade");
/*
            Tooltip.SetDefault("Enemies explode and emit healing flames on death");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 106;
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 27;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 106;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.shootSpeed = 12f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life <= 0)
            {
                int boom = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                Main.projectile[boom].Calamity().forceMelee = true;
                float randomSpeedX = (float)Main.rand.Next(5);
                float randomSpeedY = (float)Main.rand.Next(3, 7);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<PhoenixHeal>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<PhoenixHeal>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            if (target.statLife <= 0)
            {
                int boom = Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<FuckYou>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                Main.projectile[boom].Calamity().forceMelee = true;
                float randomSpeedX = (float)Main.rand.Next(5);
                float randomSpeedY = (float)Main.rand.Next(3, 7);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, -randomSpeedX, -randomSpeedY, ModContent.ProjectileType<PhoenixHeal>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI);
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, randomSpeedX, -randomSpeedY, ModContent.ProjectileType<PhoenixHeal>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f)), Item.knockBack, player.whoAmI);
            }
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 244);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BreakerBlade);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddIngredient(ItemID.SoulofSight, 3);
            recipe.AddIngredient(ItemID.SoulofFright, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
