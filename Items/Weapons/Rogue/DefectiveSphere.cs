using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace CalRD.Items.Weapons.Rogue
{
	public class DefectiveSphere : RogueWeapon
    {
        public static int BaseDamage = 130;
        public static float Speed = 15f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Defective Sphere");
/*
            Tooltip.SetDefault("Fires a variety of deadly spheres with different effects\n"
                               +"Stacks up to 5\n"
                               +"Stealth strikes launch all 4 sphere types at once");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 42;
            Item.height = 44;
            Item.damage = BaseDamage;
            Item.knockBack = 5f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.maxStack = 5;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item15; //phaseblade sound effect

            Item.value = Item.buyPrice(0, 16, 0, 0);
            Item.rare = 8;

            Item.Calamity().rogue = true;
            Item.shoot = ProjectileType<SphereSpiked>();
            Item.shootSpeed = Speed;
        }

        public override bool CanUseItem(Player player)
        {
			int UseMax = Item.stack;

			if (player.Calamity().StealthStrikeAvailable())
			{
				return true;
			}
			else if ((player.ownedProjectileCounts[Item.shoot] + player.ownedProjectileCounts[ProjectileType<SphereBladed>()] + player.ownedProjectileCounts[ProjectileType<SphereYellow>()] + player.ownedProjectileCounts[ProjectileType<SphereBlue>()]) >= UseMax)
			{
				return false;
			}
			else
			{
				return true;
			}
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int sphereType = Utils.SelectRandom(Main.rand, new int[]
			{
				type,
				ProjectileType<SphereBladed>(),
				ProjectileType<SphereYellow>(),
				ProjectileType<SphereBlue>()
			});

			//Kinda ugly but idk how to make it cleaner
			float SpeedX = velocity.X + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedY = velocity.Y + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedX2 = velocity.X + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedY2 = velocity.Y + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedX3 = velocity.X + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedY3 = velocity.Y + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedX4 = velocity.X + Main.rand.NextFloat(-30, 30) * 0.05f;
			float SpeedY4 = velocity.Y + Main.rand.NextFloat(-30, 30) * 0.05f;

            if (player.Calamity().StealthStrikeAvailable())
			{
				int stealth = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage / 2, Item.knockBack, player.whoAmI, 0f, 0f);
				int stealth2 = Projectile.NewProjectile(source, position.X, position.Y, SpeedX2, SpeedY2, ProjectileType<SphereBladed>(), damage / 2, Item.knockBack, player.whoAmI, 0f, 0f);
				int stealth3 = Projectile.NewProjectile(source, position.X, position.Y, SpeedX3, SpeedY3, ProjectileType<SphereYellow>(), damage / 2, Item.knockBack, player.whoAmI, 0f, 0f);
				int stealth4 = Projectile.NewProjectile(source, position.X, position.Y, SpeedX4, SpeedY4, ProjectileType<SphereBlue>(), damage / 2, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[stealth].Calamity().stealthStrike = true;
				Main.projectile[stealth2].Calamity().stealthStrike = true;
				Main.projectile[stealth3].Calamity().stealthStrike = true;
				Main.projectile[stealth4].Calamity().stealthStrike = true;
			}
			else
			{
				Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), sphereType, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			}
            return false;
        }
    }
}
