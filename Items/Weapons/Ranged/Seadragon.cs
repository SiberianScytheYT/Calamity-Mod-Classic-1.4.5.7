using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Seadragon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seadragon");
/*
            Tooltip.SetDefault("50% chance to not consume ammo\n" +
                "Has a chance to fire homing rockets that explode into fire shards on death");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 108;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 78;
            Item.height = 26;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (Main.rand.NextBool(10))
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<SeaDragonRocket>(), (int)(damage * 1.5), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 50)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Megalodon>());
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 9);
            recipe.AddIngredient(ModContent.ItemType<ArmoredShell>(), 3);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
