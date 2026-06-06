using CalRD.CalPlayer;
using CalRD.Items.Ammo.FiniteUse;
using CalRD.Projectiles.Typeless.FiniteUse;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Typeless.FiniteUse
{
    public class Hydra : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Hydra");
/*
            Tooltip.SetDefault("Uses Explosive Shotgun Shells\n" +
                "Does more damage to everything\n" +
                "Can be used once per boss battle");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.width = 66;
            Item.height = 30;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/Hydra");
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<ExplosiveShotgunShell>();
            Item.useAmmo = ModContent.ItemType<ExplosiveShells>();
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 1;
            }
        }

        public override bool OnPickup(Player player)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 1;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return Item.Calamity().timesUsed < 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void UpdateInventory(Player player)
        {
            if (!CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 0;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 15; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-65, 66) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-65, 66) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0.0f, 0.0f);
            }
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
				player.HeldItem.Calamity().timesUsed++;
				for (int i = 0; i < Main.InventorySlotsTotal; i++)
				{
					if (player.inventory[i].type == Item.type && player.inventory[i] != player.HeldItem)
					{
						player.inventory[i].Calamity().timesUsed++;
					}
				}
			}
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Shotgun);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddIngredient(ItemID.Ectoplasm, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
