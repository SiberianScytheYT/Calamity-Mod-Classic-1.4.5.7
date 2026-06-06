using CalRD.CalPlayer;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Fishing.AstralCatches
{
    public class PolarisParrotfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Polaris Parrotfish");
/*
            Tooltip.SetDefault("It carries the mark of the Northern Star\n" +
                "Projectile hits grant buffs to the weapon and the player\n" +
                "Buffs are removed on hit");
*/
            Item.staff[Item.type] = true; //so it doesn't look weird af when holding it
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 34;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LaserCannon"); //pew pew
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PolarStar>();
            Item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.polarisBoostThree) //Homes in and explodes
            {
                Projectile.NewProjectile(source,position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<PolarStar>(), damage, Item.knockBack, player.whoAmI, 0f, 2f);
                return false;
            }
            else if (modPlayer.polarisBoostTwo) //Splits on enemy or tile hits
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<PolarStar>(), (int)(damage * 1.25), Item.knockBack, player.whoAmI, 0f, 1f);
                return false;
            }
            return true;
        }

        public override Vector2? HoldoutOrigin() //so it looks normal when holding
        {
            return new Vector2(10, 10);
        }
    }
}
