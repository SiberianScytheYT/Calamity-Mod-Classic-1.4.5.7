using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TerraEdge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terra Edge");
/*
            Tooltip.SetDefault("Heals the player on enemy hits\n" +
                "Fires a beam that inflicts ichor for a short time");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 58;
			Item.height = 58;
			Item.scale = 1.5f;
			Item.damage = 112;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.knockBack = 6.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<TerraEdgeBeam>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * 0.75), Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrueBloodyEdge>());
            recipe.AddIngredient(ItemID.TrueExcalibur);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TrueNightsEdge);
            recipe.AddIngredient(ItemID.TrueExcalibur);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Ichor, 120);
			if (target.type == NPCID.TargetDummy || !target.canGhostHeal || player.moonLeech)
            {
                return;
            }
            int healAmount = Main.rand.Next(2) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			target.AddBuff(BuffID.Ichor, 120);
			if (player.moonLeech)
				return;
            int healAmount = Main.rand.Next(2) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }
    }
}
