using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class SDFMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("SDFMG");
/*
            Tooltip.SetDefault("It came from the edge of Terraria\n" +
                "50% chance to not consume ammo");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 74;
            Item.height = 34;
            Item.crit += 15;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.75f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-5, 6) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-5, 6) * 0.05f;
            if (Main.rand.NextBool(5))
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<FishronRPG>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
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
            recipe.AddIngredient(ItemID.SDMG);
            recipe.AddIngredient(ModContent.ItemType<FetidEmesis>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
