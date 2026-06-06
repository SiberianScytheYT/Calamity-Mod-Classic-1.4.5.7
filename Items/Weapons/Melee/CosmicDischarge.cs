using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class CosmicDischarge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cosmic Discharge");
/*
            Tooltip.SetDefault("Legendary Drop\n" +
                "Striking an enemy with the whip causes glacial explosions and grants the player the cosmic freeze buff\n" +
                "This buff gives the player increased life regen while standing still and freezes enemies near the player\n" +
                "Revengeance drop");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 52;
            Item.damage = 1000;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.5f;
            Item.UseSound = SoundID.Item122;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<CosmicDischargeFlail>();
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai3 = (Main.rand.NextFloat() - 0.75f) * 0.7853982f; //0.5
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, ai3);
            return false;
        }
    }
}
