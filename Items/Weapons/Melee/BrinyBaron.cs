using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class BrinyBaron : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Briny Baron");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
                "Striking an enemy with the blade causes a briny typhoon to appear\n" +
                "Right click to fire a razorwind aqua blade\n" +
                "Revengeance drop");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 75;
            Item.knockBack = 4f;
            Item.useAnimation = Item.useTime = 15;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.shootSpeed = 4f;

            Item.width = 100;
            Item.height = 102;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item84;
                Item.shoot = ModContent.ProjectileType<Razorwind>();
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.UseSound = SoundID.Item1;
                Item.shoot = ProjectileID.None;
            }
            return base.CanUseItem(player);
        }

		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse == 2)
				return 1f;
			//return 0.75f;
            return 1.33f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Razorwind>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187, 0f, 0f, 100, new Color(53, Main.DiscoG, 255));
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<BrinyTyphoonBubble>(), (int)(Item.damage * player.MeleeDamage() * 0.5f), Item.knockBack, player.whoAmI);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<BrinyTyphoonBubble>(), (int)(Item.damage * player.MeleeDamage() * 0.5f), Item.knockBack, player.whoAmI);
        }
    }
}
