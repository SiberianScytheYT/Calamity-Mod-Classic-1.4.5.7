using CalRD.Projectiles.Melee;
using CalRD.Projectiles.Melee.Spears;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class YateveoBloom : ModItem
    {
        public static int BaseDamage = 30;
        public static float ShootSpeed = 12f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yateveo Bloom");
/*
            Tooltip.SetDefault("A synthesis of jungle flora\n" +
                "Throws a powerful rose flail\n" +
                "Right click to stab with a flower spear");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 62;
            Item.damage = BaseDamage;
            Item.knockBack = 5f;
            Item.useAnimation = 22;
            Item.useTime = 22;

            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;

            Item.rare = 2;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(0, 2, 0, 0);

            Item.shoot = ModContent.ProjectileType<YateveoBloomProj>();
            Item.shootSpeed = ShootSpeed;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.damage = 20;
                Item.channel = false;
				Item.autoReuse = true;
                Item.useAnimation = 33;
                Item.useTime = 33;
                Item.shootSpeed = 4.5f;
				return player.ownedProjectileCounts[Item.shoot] <= 0;
			}
            else
            {
                Item.damage = BaseDamage;
                Item.channel = true;
				Item.autoReuse = false;
                Item.useAnimation = 22;
                Item.useTime = 22;
                Item.shootSpeed = ShootSpeed;
				return base.CanUseItem(player);
			}
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<YateveoBloomSpear>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            else
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<YateveoBloomProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RichMahogany, 15);
            recipe.AddIngredient(ItemID.JungleSpores, 12);
            recipe.AddIngredient(ItemID.Stinger, 4);
            recipe.AddIngredient(ItemID.Vine, 2);
            recipe.AddIngredient(ItemID.JungleRose);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
