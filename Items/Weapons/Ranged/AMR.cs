using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Weapons.Ranged
{
    public class AMR : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Anti-materiel Rifle");
/*
            Tooltip.SetDefault("Fires a .50 caliber sniper round that rips apart enemy defense and DR\n" +
                "If you crit the target a second swarm of bullets will fire near the target");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 7000;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.crit += 20;
            Item.height = 30;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LargeWeaponFire");
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Shroomer>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<AMRShot>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
