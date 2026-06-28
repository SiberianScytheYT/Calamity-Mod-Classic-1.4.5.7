using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class PrismaticBreaker : ModItem
    {
        private int alpha = 50;
		public Color[] colors = new Color[]
		{
			new Color(255, 0, 0, 50), //Red
			new Color(255, 128, 0, 50), //Orange
			new Color(255, 255, 0, 50), //Yellow
			new Color(128, 255, 0, 50), //Lime
			new Color(0, 255, 0, 50), //Green
			new Color(0, 255, 128, 50), //Turquoise
			new Color(0, 255, 255, 50), //Cyan
			new Color(0, 128, 255, 50), //Light Blue
			new Color(0, 0, 255, 50), //Blue
			new Color(128, 0, 255, 50), //Purple
			new Color(255, 0, 255, 50), //Fuschia
			new Color(255, 0, 128, 50) //Hot Pink
		};
		List<Color> colorSet = new List<Color>()
		{
			new Color(255, 0, 0, 50), //Red
			new Color(255, 255, 0, 50), //Yellow
			new Color(0, 255, 0, 50), //Green
			new Color(0, 255, 255, 50), //Cyan
			new Color(0, 0, 255, 50), //Blue
			new Color(255, 0, 255, 50), //Fuschia
		};

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Prismatic Breaker");
/*
            Tooltip.SetDefault("Seems to belong to a certain magical girl. Radiates with intense cosmic energy.\n" +
                "Fire to charge for a powerful rainbow laser\n" +
				"Right click to instead swing the sword and fire rainbow colored waves\n" +
				"The sword is boosted by both melee and ranged damage");
*/
			Item.staff[Item.type] = true;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 1200;
            Item.crit += 8;
            Item.useStyle = 1;
            Item.useTime = Item.useAnimation = 15;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.width = 50;
            Item.height = 50;
            Item.shoot = ModContent.ProjectileType<PrismaticBeam>();
            Item.shootSpeed = 14f;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

		//Cancel out normal melee damage boosts and replace it with the average of melee and ranged damage boosts
		//all damage boosts should still apply
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
			/*
			float damageMult = (player.GetDamage(DamageClass.Melee).Additive + player.GetDamage(DamageClass.Ranged).Additive - 2f) / 2f;
            damage += damageMult - player.GetDamage(DamageClass.Melee).Additive + 1f;
            */
			StatModifier halfMelee = damage.Scale(0.5f);
            damage = halfMelee.CombineWith(player.GetTotalDamage<RangedDamageClass>().Scale(0.5f));
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/PrismaticBreakerGlow").Value);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PrismaticWave>(), (int)(damage * 1), Item.knockBack, player.whoAmI, 0f, 0f);
            }
			else
			{
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 0.5f, velocity.Y * 0.5f, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			}
            return false;
        }

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.UseSound = SoundID.Item1;
				Item.useStyle = 1;
				Item.useTurn = true;
				Item.autoReuse = true;
				Item.noMelee = false;
				Item.channel = false;
			}
			else
			{
				Item.UseSound = new SoundStyle("CalRD/Sounds/Item/CrystylCharge");
				Item.useStyle = 5;
				Item.useTurn = false;
				Item.autoReuse = false;
				Item.noMelee = true;
				Item.channel = true;
			}
			return base.CanUseItem(player);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                Dust rainbow = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 267, 0f, 0f, alpha, Main.rand.Next(colors), 0.8f)];
                rainbow.noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            target.AddBuff(BuffID.CursedInferno, 120);
            target.AddBuff(BuffID.Frostburn, 120);
            target.AddBuff(BuffID.OnFire, 120);
            target.AddBuff(BuffID.Ichor, 120);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            target.AddBuff(BuffID.CursedInferno, 120);
            target.AddBuff(BuffID.Frostburn, 120);
            target.AddBuff(BuffID.OnFire, 120);
            target.AddBuff(BuffID.Ichor, 120);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmicRainbow>());
            recipe.AddIngredient(ModContent.ItemType<SolsticeClaymore>());
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
