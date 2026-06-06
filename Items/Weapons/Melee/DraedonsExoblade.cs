using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class DraedonsExoblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Exoblade");
/*
			Tooltip.SetDefault("Ancient blade of Yharim's weapons and armors expert, Draedon\n" +
							   "Fires an exo beam that homes in on the player and explodes\n" +
							   "Striking an enemy with the blade causes several comets to fire\n" +
							   "All attacks briefly freeze enemies hit\n" +
							   "Enemies hit at very low HP explode into frost energy and freeze nearby enemies\n" +
							   "The lower your HP the more damage this blade does and heals the player on enemy hits");
*/
		}

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.damage = 5000;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 114;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<Exobeam>();
            Item.shootSpeed = 19f;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        // Gains 100% of missing health as base damage.
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			int lifeAmount = player.statLifeMax2 - player.statLife;
			damage.Base += lifeAmount; // * player.MeleeDamage();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(4))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 107, 0f, 0f, 100, new Color(0, 255, 255));
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (target.life <= (target.lifeMax * 0.05f))
			{
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<Exoboom>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
			}
			target.ExoDebuffs();
			SoundEngine.PlaySound(SoundID.Item88, player.Center);
			float xPos = player.position.X + 800 * Main.rand.NextBool(2).ToDirectionInt();
			float yPos = player.position.Y + Main.rand.Next(-800, 801);
			Vector2 startPos = new Vector2(xPos, yPos);
			Vector2 velocity = target.position - startPos;
			float dir = 10 / startPos.X;
			velocity.X *= dir * 150;
			velocity.Y *= dir * 150;
			velocity.X = MathHelper.Clamp(velocity.X, -15f, 15f);
			velocity.Y = MathHelper.Clamp(velocity.Y, -15f, 15f);
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Exocomet>()] < 8)
			{
				for (int comet = 0; comet < 2; comet++)
				{
					float ai1 = Main.rand.NextFloat() + 0.5f;
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), startPos, velocity, ModContent.ProjectileType<Exocomet>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI, 0f, ai1);
				}
			}
			if (target.type == NPCID.TargetDummy || !target.canGhostHeal || player.moonLeech)
			{
				return;
			}
			int healAmount = Main.rand.Next(5) + 5;
			player.statLife += healAmount;
			player.HealEffect(healAmount);
		}

		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
		{
			if (target.statLife <= (target.statLifeMax2 * 0.05f))
			{
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<Exoboom>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, Main.myPlayer);
			}
			target.ExoDebuffs();
			SoundEngine.PlaySound(SoundID.Item88, player.Center);
			float xPos = player.position.X + 800 * Main.rand.NextBool(2).ToDirectionInt();
			float yPos = player.position.Y + Main.rand.Next(-800, 801);
			Vector2 startPos = new Vector2(xPos, yPos);
			Vector2 velocity = target.position - startPos;
			float dir = 10 / startPos.X;
			velocity.X *= dir * 150;
			velocity.Y *= dir * 150;
			velocity.X = MathHelper.Clamp(velocity.X, -15f, 15f);
			velocity.Y = MathHelper.Clamp(velocity.Y, -15f, 15f);
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Exocomet>()] < 8)
			{
				for (int comet = 0; comet < 2; comet++)
				{
					float ai1 = Main.rand.NextFloat() + 0.5f;
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), startPos, velocity, ModContent.ProjectileType<Exocomet>(), (int)(Item.damage * player.MeleeDamage()), Item.knockBack, player.whoAmI, 0f, ai1);
				}
			}
			if (player.moonLeech)
				return;
			int healAmount = Main.rand.Next(5) + 5;
			player.statLife += healAmount;
			player.HealEffect(healAmount);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Terratomere>());
			recipe.AddIngredient(ModContent.ItemType<AnarchyBlade>());
			recipe.AddIngredient(ModContent.ItemType<FlarefrostBlade>());
			recipe.AddIngredient(ModContent.ItemType<PhoenixBlade>());
			recipe.AddIngredient(ModContent.ItemType<StellarStriker>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
