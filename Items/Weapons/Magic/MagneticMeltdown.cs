using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class MagneticMeltdown : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magnetic Meltdown");
/*
            Tooltip.SetDefault("Fires a spread of magnetic orbs");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 25;
            Item.width = 78;
            Item.height = 78;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MagneticOrb>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 4; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-50, 51) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-50, 51) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 1f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ItemID.SpectreStaff);
            recipe.AddIngredient(ItemID.MagnetSphere);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
