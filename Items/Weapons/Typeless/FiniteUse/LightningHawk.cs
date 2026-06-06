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
    public class LightningHawk : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lightning Hawk");
/*
            Tooltip.SetDefault("Uses Magnum Rounds\n" +
                "Does more damage to organic enemies\n" +
                "Can be used thrice per boss battle");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.crit += 56;
            Item.width = 50;
            Item.height = 28;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/Magnum");
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<MagnumRound>();
            Item.useAmmo = ModContent.ItemType<MagnumRounds>();
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 3;
            }
        }

        public override bool OnPickup(Player player)
        {
            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Item.Calamity().timesUsed = 3;
            }
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return Item.Calamity().timesUsed < 3;
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
            recipe.AddIngredient(ModContent.ItemType<Magnum>());
            recipe.AddIngredient(ItemID.SoulofMight, 20);
            recipe.AddIngredient(ItemID.SoulofSight, 20);
            recipe.AddIngredient(ItemID.SoulofFright, 20);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
