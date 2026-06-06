using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class OnyxChainBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Onyx Chain Blaster");
/*
            Tooltip.SetDefault("50% chance to not consume ammo\n" +
                "Fires a spread of bullets and an onyx shard");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 24f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-25, 26) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-25, 26) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX * 0.9f, SpeedY * 0.9f, ProjectileID.BlackBolt, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            for (int i = 0; i <= 3; i++)
            {
                float SpeedNewX = velocity.X + (float)Main.rand.Next(-45, 46) * 0.05f;
                float SpeedNewY = velocity.Y + (float)Main.rand.Next(-45, 46) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedNewX, SpeedNewY, type, (int)(damage * 1.25f), Item.knockBack, player.whoAmI, 0f, 0f);
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
            recipe.AddIngredient(ItemID.OnyxBlaster);
            recipe.AddIngredient(ItemID.ChainGun);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
