using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class UtensilPoker : RogueWeapon
    {
		private int counter = 0;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Utensil Poker");
/*
            Tooltip.SetDefault("Space chickens, that is all.\n" +
				"Fires random utensils in bursts of three\n" +
                "Grants Well Fed on enemy hits\n" +
                "Stealth strikes launch an additional butcher knife");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 44;
            Item.height = 66;
            Item.damage = 540;
            Item.Calamity().rogue = true;
            Item.knockBack = 8f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.reuseDelay = 15;
            Item.useAnimation = 45;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.shoot = ModContent.ProjectileType<Fork>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable() && counter == 0)
            {
                int stealth = Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.2f, velocity.Y * 1.2f, ModContent.ProjectileType<ButcherKnife>(), (int)(damage * 1.4), Item.knockBack, player.whoAmI);
                Main.projectile[stealth].Calamity().stealthStrike = true;
            }
			int utensil = Item.shoot;
			double dmgMult = 1D;
			float kbMult = 1f;
			switch (Main.rand.Next(3))
			{
				case 0:
					utensil = Item.shoot;
					dmgMult = 1.1;
					kbMult = 2f;
					break;
				case 1:
					utensil = ModContent.ProjectileType<Knife>();
					dmgMult = 1.2;
					kbMult = 1f;
					break;
				case 2:
					utensil = ModContent.ProjectileType<CarvingFork>();
					dmgMult = 1D;
					kbMult = 1f;
					break;
                default:
                    break;
			}
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), utensil, (int)(damage * dmgMult), Item.knockBack * kbMult, player.whoAmI);

			counter++;
			if (counter >= 3)
				counter = 0;
            return false;
        }
    }
}
