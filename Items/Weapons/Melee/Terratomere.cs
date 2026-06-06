using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Terratomere : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Terratomere");
/*
            Tooltip.SetDefault("Linked to the essence of Terraria\n" +
                               "Heals the player on true melee hits\n" +
                               "Fires a barrage of homing beams that freeze enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.damage = 125;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 21;
            Item.useTurn = true;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<TerratomereProjectile>();
            Item.shootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(4, 6);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(damage * 0.5), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Floodtide>());
            recipe.AddIngredient(ModContent.ItemType<Hellkite>());
            recipe.AddIngredient(ModContent.ItemType<TemporalFloeSword>());
            recipe.AddIngredient(ItemID.TerraBlade);
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Floodtide>());
            recipe.AddIngredient(ModContent.ItemType<Hellkite>());
            recipe.AddIngredient(ModContent.ItemType<TemporalFloeSword>());
            recipe.AddIngredient(ModContent.ItemType<TerraEdge>());
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            if (target.type == NPCID.TargetDummy || !target.canGhostHeal || player.moonLeech)
            {
                return;
            }
            int healAmount = Main.rand.Next(3) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
			if (player.moonLeech)
				return;
            int healAmount = Main.rand.Next(3) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }
    }
}
