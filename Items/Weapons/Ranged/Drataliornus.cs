using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Drataliornus : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Drataliornus");
/*
            Tooltip.SetDefault("Fires an escalating stream of fireballs.\n"
                               +"Fireballs rain meteors, leave dragon dust trails, and launch additional bolts at max speed.\n"
                               +"Taking damage while firing the stream will interrupt it and reduce your wing flight time.\n"
                               +"Right click to fire two devastating barrages of five empowered fireballs.\n"
                               +"'Just don't get hit'");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 700;
            Item.knockBack = 1f;
            Item.shootSpeed = 18f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.useTime = 12;
            Item.reuseDelay = 48;
            Item.width = 64;
            Item.height = 84;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ModContent.ProjectileType<DrataliornusBow>();
            Item.value = Item.buyPrice(platinum: 2, gold: 50);
            Item.rare = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useTurn = false;
            Item.useAmmo = AmmoID.Arrow;
            Item.autoReuse = true;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = false;
            }
            else
            {
                Item.noUseGraphic = true;
				if (player.ownedProjectileCounts[Item.shoot] > 0)
				{
                    return false;
				}
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) //tsunami
            {
                const float num3 = 0.471238898f;
                const int num4 = 5;
                Vector2 spinningpoint = new Vector2(velocity.X, velocity.Y);
                spinningpoint.Normalize();
                spinningpoint *= 36f;
                for (int index1 = 0; index1 < num4; ++index1)
                {
                    float num8 = index1 - (num4 - 1) / 2;
                    Vector2 vector2_5 = spinningpoint.RotatedBy(num3 * num8, new Vector2());
                    Projectile.NewProjectile(source, position.X + vector2_5.X, position.Y + vector2_5.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DrataliornusFlame>(), (int)(damage * 0.69), Item.knockBack, player.whoAmI, 1f, 0f);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<DrataliornusBow>(), 0, 0f, player.whoAmI);
            }

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(4f, 0f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BlossomFlux>());
            recipe.AddIngredient(ModContent.ItemType<DaemonsFlame>());
            recipe.AddIngredient(ModContent.ItemType<Deathwind>());
            recipe.AddIngredient(ModContent.ItemType<HeavenlyGale>());
            recipe.AddIngredient(ModContent.ItemType<DragonsBreath>(), 2);
            recipe.AddIngredient(ModContent.ItemType<ChickenCannon>(), 2);
            recipe.AddIngredient(ModContent.ItemType<AngryChickenStaff>(), 2);
            recipe.AddIngredient(ModContent.ItemType<YharimsGift>(), 3);
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 60);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
