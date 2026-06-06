using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class SarosPossession : ModItem
    {
		int radianceSlots;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Saros Possession");
/*
            Tooltip.SetDefault("Gain absolute control over light itself\n" +
							   "Summons a radiant aura\n" +
                               "Consumes all of the remaining minion slots on use\n" +
							   "Must be used from the hotbar\n" +
                               "Increased power based on the number of minion slots used");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 56;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.UseSound = SoundID.DD2_BetsyFlameBreath;

            Item.DamageType = DamageClass.Summon;
            Item.mana = 100;
            Item.damage = 426;
            Item.knockBack = 4f;
            Item.useTime = Item.useAnimation = 10;
            Item.shoot = ModContent.ProjectileType<RadiantResolutionAura>();
            Item.shootSpeed = 10f;

            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

		public override void HoldItem(Player player)
        {
			double minionCount = 0;
			for (int j = 0; j < Main.projectile.Length; j++)
			{
                Projectile proj = Main.projectile[j];
				if (proj.active && proj.owner == player.whoAmI && proj.minion && proj.type != Item.shoot)
				{
					minionCount += proj.minionSlots;
				}
			}
            radianceSlots = (int)(player.maxMinions - minionCount);
		}

        public override bool CanUseItem(Player player)
		{
			return radianceSlots >= 1;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityUtils.KillShootProjectiles(true, type, player);
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, radianceSlots);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Sirius>());
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 50);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
