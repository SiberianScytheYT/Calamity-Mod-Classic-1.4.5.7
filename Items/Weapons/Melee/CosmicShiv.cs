using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using CalRD.Projectiles.Melee;

namespace CalRD.Items.Weapons.Melee
{
	public class CosmicShiv : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cosmic Shiv");
/*
			Tooltip.SetDefault("Definitely don't underestimate the power of shivs\n" +
							   "Fires a cosmic beam that homes in on enemies\n" +
                               "Upon hitting an enemy, a barrage of offscreen objects home in on the enemy as well as raining stars");
*/
		}

		public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTurn = false;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.width = 44;
			Item.height = 44;

			Item.damage = 666;

			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.knockBack = 9f;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CosmicShivBall>();
			Item.shootSpeed = 14f;
			Item.value = Item.buyPrice(2, 50, 0, 0); //50 gold
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, Item.shootSpeed * player.direction, 0f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe.AddIngredient(ModContent.ItemType<ElementalShortsword>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
	        recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int k = 0; k < 36; k++)
            {
                int dustID = Dust.NewDust(new Vector2(player.position.X, player.position.Y + 16f), player.width, player.height - 16, 173, 0f, 0f, 0, default, 1f);
                Main.dust[dustID].velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }

            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 420);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 420);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 420);
            target.AddBuff(ModContent.BuffType<Plague>(), 420);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 360);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            for (int k = 0; k < 36; k++)
            {
                int dustID = Dust.NewDust(new Vector2(player.position.X, player.position.Y + 16f), player.width, player.height - 16, 173, 0f, 0f, 0, default, 1f);
                Main.dust[dustID].velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }

            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 420);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 420);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 420);
            target.AddBuff(ModContent.BuffType<Plague>(), 420);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 360);
        }
	}
}
