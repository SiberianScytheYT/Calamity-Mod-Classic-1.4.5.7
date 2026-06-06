using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class ElementalExcalibur : ModItem
    {
        private static int BaseDamage = 10000;
        private int BeamType = 0;
        private int alpha = 50;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Elemental Excalibur");
/*
            Tooltip.SetDefault("Freezes enemies and heals the player on hit\n" +
                "Fires rainbow beams that change their behavior based on their color\n" +
				"Right click for true melee");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.crit += 10;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.width = 92;
            Item.height = 92;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ElementalExcaliburBeam>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.Rainbow;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/ElementalExcaliburGlow").Value);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, (float)BeamType, 0f);

            BeamType++;
            if (BeamType > 11)
                BeamType = 0;

            return false;
        }

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = ProjectileID.None;
				Item.shootSpeed = 0f;
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<ElementalExcaliburBeam>();
				Item.shootSpeed = 12f;
			}
			return base.CanUseItem(player);
		}

		public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (player.altFunctionUse == 2)
                modifiers.SourceDamage *= 2;
		}

		public override void ModifyHitPvp(Player player, Player target, ref Player.HurtModifiers modifiers)
		{
			if (player.altFunctionUse == 2)
                modifiers.SourceDamage *= 2;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                Color color = new Color(255, 0, 0, alpha);
                switch (BeamType)
                {
                    case 0: // Red
                        break;
                    case 1: // Orange
                        color = new Color(255, 128, 0, alpha);
                        break;
                    case 2: // Yellow
                        color = new Color(255, 255, 0, alpha);
                        break;
                    case 3: // Lime
                        color = new Color(128, 255, 0, alpha);
                        break;
                    case 4: // Green
                        color = new Color(0, 255, 0, alpha);
                        break;
                    case 5: // Turquoise
                        color = new Color(0, 255, 128, alpha);
                        break;
                    case 6: // Cyan
                        color = new Color(0, 255, 255, alpha);
                        break;
                    case 7: // Light Blue
                        color = new Color(0, 128, 255, alpha);
                        break;
                    case 8: // Blue
                        color = new Color(0, 0, 255, alpha);
                        break;
                    case 9: // Purple
                        color = new Color(128, 0, 255, alpha);
                        break;
                    case 10: // Fuschia
                        color = new Color(255, 0, 255, alpha);
                        break;
                    case 11: // Hot Pink
                        color = new Color(255, 0, 128, alpha);
                        break;
                    default:
                        break;
                }

                Dust dust24 = Main.dust[Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 267, 0f, 0f, alpha, color, 1.2f)];
                dust24.noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ExoFreeze>(), 60);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 240);
            target.AddBuff(ModContent.BuffType<Plague>(), 240);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 240);
            target.AddBuff(BuffID.CursedInferno, 240);
            target.AddBuff(BuffID.Frostburn, 240);
            target.AddBuff(BuffID.OnFire, 240);
            target.AddBuff(BuffID.Ichor, 240);
            if (target.type == NPCID.TargetDummy || !target.canGhostHeal || player.moonLeech)
            {
                return;
            }
            int healAmount = Main.rand.Next(10) + 10;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<ExoFreeze>(), 60);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 240);
            target.AddBuff(ModContent.BuffType<Plague>(), 240);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 240);
            target.AddBuff(BuffID.CursedInferno, 240);
            target.AddBuff(BuffID.Frostburn, 240);
            target.AddBuff(BuffID.OnFire, 240);
            target.AddBuff(BuffID.Ichor, 240);
			if (player.moonLeech)
				return;
            int healAmount = Main.rand.Next(10) + 10;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GreatswordofBlah>());
            recipe.AddIngredient(ItemID.TrueExcalibur);
            recipe.AddIngredient(ItemID.LargeDiamond, 3);
            recipe.AddIngredient(ItemID.LightShard, 10);
            recipe.AddIngredient(ItemID.DarkShard, 10);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 10);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 10);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 10);
            recipe.AddIngredient(ItemID.SoulofLight, 50);
            recipe.AddIngredient(ItemID.SoulofNight, 50);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
