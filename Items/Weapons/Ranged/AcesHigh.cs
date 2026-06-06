using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class AcesHigh : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ace's High");
/*
            Tooltip.SetDefault("Fires a string of cards with varying effects based on card type\n" +
			"Hearts grant lifesteal. Spades pierce and ignore immunity frames.\n" +
			"Diamonds explode. Clubs split into three.");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 1599;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 48;
            Item.height = 30;
            Item.useTime = 3;
            Item.reuseDelay = 8;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.shootSpeed = 24f;
            Item.shoot = ModContent.ProjectileType<CardHeart>();
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ClaretCannon>());
            recipe.AddIngredient(ModContent.ItemType<Spyker>());
            recipe.AddIngredient(ModContent.ItemType<Fungicide>());
            recipe.AddIngredient(ModContent.ItemType<FantasyTalisman>(), 52);
            recipe.AddIngredient(ModContent.ItemType<HellcasterFragment>(), 6);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int card = Utils.SelectRandom(Main.rand, new int[]
			{
				ModContent.ProjectileType<CardHeart>(),
				ModContent.ProjectileType<CardSpade>(),
				ModContent.ProjectileType<CardDiamond>(),
				ModContent.ProjectileType<CardClub>()
			});

            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, card, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
