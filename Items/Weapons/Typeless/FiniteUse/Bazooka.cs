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
    public class Bazooka : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bazooka");
/*
            Tooltip.SetDefault("Uses Grenade Shells\n" +
                "Does more damage to inorganic enemies\n" +
                "Can be used twice per boss battle");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 500;
            Item.width = 66;
            Item.height = 26;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/BazookaFull");
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<GrenadeRound>();
            Item.useAmmo = ModContent.ItemType<GrenadeRounds>();
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 2;
            }
        }

        public override bool OnPickup(Player player)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 2;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return Item.Calamity().timesUsed < 2;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
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
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddRecipeGroup("AnyAdamantiteBar", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
